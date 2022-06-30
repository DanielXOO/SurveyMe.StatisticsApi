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

namespace Statistics.Services;

public class StatisticsService : IStatisticsService
{
    private readonly IStatisticsUnitOfWork _unitOfWork;

    private readonly IPersonsApi _personsApi;

    private readonly IMapper _mapper;
    
    
    public StatisticsService(IStatisticsUnitOfWork unitOfWork, IPersonsApi personsApi, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _personsApi = personsApi;
        _mapper = mapper;
    }
    
    
    public async Task CreateStatisticsAsync(Survey survey)
    {
        var surveyStatistics = new SurveyStatistics
        {
            SurveyId = survey.Id,
            Personalities = new List<Personality>(),
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
        }

        surveyStatistics.QuestionStatistics = questionsStatistics;

        await _unitOfWork.Statistics.CreateAsync(surveyStatistics);
    }

    public async Task UpdateStatisticsAsync(SurveyAnswer answer)
    {
        
    }
}