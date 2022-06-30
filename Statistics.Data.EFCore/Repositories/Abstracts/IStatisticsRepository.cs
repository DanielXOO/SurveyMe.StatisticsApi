using Statistics.Data.EFCore.Core.Abstracts;
using Statistics.Models.Statistics;
using Statistics.Models.Statistics.Surveys;

namespace Statistics.Data.EFCore.Repositories.Abstracts;

public interface IStatisticsRepository : IRepository<SurveyStatistics>
{
    Task<SurveyStatistics> GetStatisticById(Guid id);
    
    Task<SurveyStatistics> GetStatisticBySurveyId(Guid surveyId);

    Task<bool> IsStatisticsExists(Guid surveyId);
}