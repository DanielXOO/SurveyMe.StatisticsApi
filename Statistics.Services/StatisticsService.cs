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
using SurveyMe.PersonsApi.Models.Request.Personality;
using SurveyMe.SurveyPersonApi.Models.Common;

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

        if (statistics == null)
        {
            throw new NotFoundException("Statistics not found");
        }
        
        var personalityOptionsResponse = await _personOptionsApi
            .GetSurveyOptionsAsync(statistics.SurveyId);

        if (personalityOptionsResponse == null)
        {
            throw new NotFoundException("Options for survey not found");
        }

        var personalityOptions = personalityOptionsResponse.Options
            .Where(o => o.IsRequired)
            .Select(o => o.PropertyName)
            .ToList();

        var personalities = new List<Personality>();
        
        foreach (var statisticsPersonality in statistics.Personalities)
        {
            var personalityResponse = await _personsApi
                .GetPersonalityAsync(statisticsPersonality.PersonalityId, surveyId, personalityOptions);
            
            var personality = _mapper.Map<Personality>(personalityResponse);
            
            personalities.Add(personality);
        }

        var statisticResult = new SurveyStatisticsWithPersonality
        {
            SurveyStatistics = statistics
        };
        
        if (personalityOptions.Contains(PropertyNames.Age))
        {
            var averageAge = personalities.Sum(p => p.Age) / personalities.Count;
            statisticResult.AverageAge = averageAge;
        }

        if (personalityOptions.Contains(PropertyNames.Gender))
        {
            var genderStatistics = personalities
                .GroupBy(p => p.Gender)
                .ToDictionary(grouping => grouping.Key.Value, grouping => grouping.Count());

            statisticResult.GenderStatistics = genderStatistics;
        }
        
        return statisticResult;
    }

    public async Task DeleteStatisticsAsync(Survey survey)
    {
        var isExists = await _unitOfWork.Statistics.IsStatisticsExists(survey.Id);

        if (!isExists)
        {
            throw new NotFoundException("Statistics do not exist");
        }
        
        await _unitOfWork.Statistics.DeleteStatisticsBySurveyIdAsync(survey.Id);
    }

    public async Task UpdateStatisticsAsync(Survey survey)
    {
        await DeleteStatisticsAsync(survey);

        await CreateStatisticsAsync(survey);
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

        if (statistics == null)
        {
            throw new NotFoundException("Statistics not found");
        }
        
        statistics.AnswersCount++;

        var personality = new PersonalityInfo
        {
            PersonalityId = answer.PersonalityId
        };
        
        statistics.Personalities.Add(personality);
        
        foreach (var questionAnswer in answer.Answers)
        {
            var questionStatistics = statistics.QuestionStatistics
                .FirstOrDefault(q => q.QuestionId == questionAnswer.QuestionId);

            if (questionStatistics == null)
            {
                throw new NotFoundException("Question not found");
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
                        throw new NotFoundException("Option not found");
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
                            throw new NotFoundException("Option not found");
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

        await _unitOfWork.Statistics.UpdateAsync(statistics);
    }
}