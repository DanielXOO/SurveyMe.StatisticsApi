using AutoMapper;
using Statistics.Data.EFCore.Abstracts;
using Statistics.Data.Refit;
using Statistics.Models.Answers;
using Statistics.Models.Personalities;
using Statistics.Models.Statistics.Options;
using Statistics.Models.Statistics.Questions;
using Statistics.Models.Statistics.Surveys;
using Statistics.Models.Surveys;
using Statistics.Services.Abstracts;
using Statistics.Services.Models;
using SurveyMe.Common.Exceptions;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;

namespace Statistics.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsUnitOfWork _unitOfWork;

    private readonly IPersonsApi _personsApi;

    private readonly ISurveyPersonOptionsApi _personOptionsApi;

    private readonly IMapper _mapper;
    
    
    public StatisticsService(IStatisticsUnitOfWork unitOfWork, IPersonsApi personsApi,
        IMapper mapper, ISurveyPersonOptionsApi personOptionsApi)
    {
        _unitOfWork = unitOfWork;
        _personsApi = personsApi;
        _mapper = mapper;
        _personOptionsApi = personOptionsApi;
    }


    public async Task<SurveyStatisticsWithPersonality> GetStatisticsBySurveyId(Guid surveyId)
    {
        var statistics = await _unitOfWork.Statistics.GetStatisticBySurveyId(surveyId);

        var personalityOptionsResponse = await _personOptionsApi
            .GetSurveyOptionsAsync(statistics.SurveyId);

        var personalityOptions = _mapper.Map<SurveyOptionsGetRequestModel>(personalityOptionsResponse);

        var personalities = new List<Personality>();
        
        foreach (var statisticsPersonality in statistics.Personalities)
        {
            var personalityResponse = await _personsApi
                .GetPersonalityAsync(statisticsPersonality.Id, personalityOptions);
            var personality = _mapper.Map<Personality>(personalityResponse);
            
            personalities.Add(personality);
        }

        var averageAge = personalities.Sum(p => p.Age) / personalities.Count;

        var genderStatistics = personalities
            .GroupBy(p => p.Gender)
            .ToDictionary(grouping => grouping.Key.Value, grouping => grouping.Count());

        var statisticResult = new SurveyStatisticsWithPersonality
        {
            GenderStatistics = genderStatistics,
            AverageAge = averageAge,
            SurveyStatistics = statistics
        };

        return statisticResult;
    }

    public async Task DeleteStatisticsAsync(Survey survey)
    {
        await _unitOfWork.Statistics.DeleteStatisticsBySurveyIdAsync(survey.Id);
    }

    public async Task CreateStatisticsAsync(Survey survey)
    {
        var surveyStatistics = new SurveyStatistics
        {
            SurveyId = survey.Id,
            Personalities = new List<PersonalityInfo>(),
        };

        var questionsStatistics = new List<BaseQuestionStatistics>();
        
        foreach (var question in survey.Questions)
        {
            BaseQuestionStatistics questionStatistics = question.Type switch
            {
                QuestionType.Text => new TextQuestionStatistics(),
                QuestionType.Radio => new RadioQuestionStatistics
                {
                    OptionsStatistics = question.QuestionsOptions.Select(o 
                        => new RadioOptionAnswerStatistics
                        {
                            OptionId = o.Id
                        }).ToList()
                },
                QuestionType.Checkbox => new CheckboxQuestionStatistics
                {
                    OptionsStatistics = question.QuestionsOptions.Select(o 
                        => new CheckboxOptionAnswerStatistics()
                        {
                            OptionId = o.Id
                        }).ToList()
                },
                QuestionType.File => new FileQuestionStatistics(),
                QuestionType.Rate => new RateQuestionStatistics(),
                QuestionType.Scale => new ScaleQuestionStatistics(),
                _ => throw new ArgumentOutOfRangeException()
            };

            questionStatistics.QuestionId = question.Id;
            questionStatistics.QuestionType = question.Type;
            
            questionsStatistics.Add(questionStatistics);
        }

        surveyStatistics.QuestionStatistics = questionsStatistics;

        await _unitOfWork.Statistics.CreateAsync(surveyStatistics);
    }

    public async Task AddAnswerToStatisticsAsync(SurveyAnswer answer)
    {
        var statistics = await _unitOfWork.Statistics.GetStatisticBySurveyId(answer.SurveyId);

        statistics.AnswersCount++;
        foreach (var questionAnswer in answer.QuestionAnswers)
        {
            var questionStatistics = statistics.QuestionStatistics
                .FirstOrDefault(q => q.QuestionId == questionAnswer.QuestionId);

            if (questionStatistics == null)
            {
                throw new BadRequestException("Question not found");
            }
            
            switch (questionAnswer)
            {
                case TextQuestionAnswer:
                    break;
                case RadioQuestionAnswer radioAnswer:
                    var option = ((RadioQuestionStatistics) questionStatistics).OptionsStatistics
                        .FirstOrDefault(o => o.OptionId == radioAnswer.OptionId);

                    if (option == null)
                    {
                        throw new BadRequestException("Option not found");
                    }
                    
                    option.AnswersCount++;
                    break;
                case CheckboxQuestionAnswer checkboxAnswer:
                    foreach (var optionAnswer in checkboxAnswer.Options)
                    {
                        var optionStatistic = ((CheckboxQuestionStatistics) questionStatistics).OptionsStatistics
                            .FirstOrDefault(o => o.OptionId == optionAnswer.OptionId);

                        if (optionStatistic == null)
                        {
                            throw new BadRequestException("Option not found");
                        }
                        
                        optionStatistic.AnswersCount++;
                    }
                    break;
                case FileQuestionAnswer:
                    break;
                case RateQuestionAnswer rateAnswer:
                    var rateStatistic = (RateQuestionStatistics) questionStatistics;
                    var rateSum =  rateStatistic.AverageRate * questionStatistics.AnswersCount + rateAnswer.Rate;
                    rateStatistic.AverageRate = rateSum / (rateStatistic.AnswersCount + 1);
                    break;
                case ScaleQuestionAnswer scaleAnswer:
                    var scaleStatistic = (ScaleQuestionStatistics) questionStatistics;
                    var scaleSum =  scaleStatistic.AverageScale * 
                        questionStatistics.AnswersCount + scaleAnswer.Scale;
                    scaleStatistic.AverageScale = scaleSum / (scaleStatistic.AnswersCount + 1);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            questionStatistics.AnswersCount++;
        }
    }
}