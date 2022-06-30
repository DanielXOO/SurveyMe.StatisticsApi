using Statistics.Models.Answers;
using Statistics.Models.Surveys;

namespace Statistics.Services.Abstracts;

public interface IStatisticsService
{
    //TODO: Add Get Delete

    Task CreateStatisticsAsync(Survey survey);
    
    Task UpdateStatisticsAsync(SurveyAnswer answer);
}