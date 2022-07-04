using Statistics.Models.Answers;
using Statistics.Models.Surveys;
using Statistics.Services.Models;

namespace Statistics.Services.Abstracts;

public interface IStatisticsService
{
    Task<SurveyStatisticsWithPersonality> GetStatisticsBySurveyId(Guid surveyId);

    Task DeleteStatisticsAsync(Survey survey);
    
    Task CreateStatisticsAsync(Survey survey);
    
    Task AddAnswerToStatisticsAsync(SurveyAnswer answer);
}