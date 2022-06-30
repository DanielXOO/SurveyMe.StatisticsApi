using Microsoft.EntityFrameworkCore;
using Statistics.Data.EFCore.Abstracts;
using Statistics.Data.EFCore.Core;
using Statistics.Data.EFCore.Repositories.Abstracts;
using Statistics.Models.Statistics;
using Statistics.Models.Statistics.Surveys;

namespace Statistics.Data.EFCore;

public class StatisticsUnitOfWork : UnitOfWork, IStatisticsUnitOfWork
{
    public IStatisticsRepository Statistics
        => (IStatisticsRepository)GetRepository<SurveyStatistics>();

    protected StatisticsUnitOfWork(DbContext dbContext) : base(dbContext)
    {
        AddSpecificRepository<SurveyStatistics, IStatisticsRepository>();
    }
}