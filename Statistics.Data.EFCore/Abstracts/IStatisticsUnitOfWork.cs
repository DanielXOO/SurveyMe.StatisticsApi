using Statistics.Data.EFCore.Core.Abstracts;
using Statistics.Data.EFCore.Repositories.Abstracts;

namespace Statistics.Data.EFCore.Abstracts;

public interface IStatisticsUnitOfWork : IUnitOfWork
{
    IStatisticsRepository Statistics { get; }
}