using Microsoft.EntityFrameworkCore;
using Statistics.Data.EFCore.Core;
using Statistics.Data.EFCore.Repositories.Abstracts;
using Statistics.Models.Statistics.Questions;
using Statistics.Models.Statistics.Surveys;

namespace Statistics.Data.EFCore.Repositories;

public class StatisticsRepository : Repository<SurveyStatistics>, IStatisticsRepository
{
    public StatisticsRepository(DbContext dbContext) : base(dbContext) { }
    
    
    public async Task<SurveyStatistics> GetStatisticById(Guid id)
    {
        var statistic = await GetStatisticQuery()
            .FirstOrDefaultAsync(s => s.Id == id);

        return statistic;
    }

    public async Task<SurveyStatistics> GetStatisticBySurveyId(Guid surveyId)
    {
        var statistic = await GetStatisticQuery()
            .FirstOrDefaultAsync(s => s.SurveyId == surveyId);

        return statistic;
    }

    public async Task DeleteStatisticsBySurveyIdAsync(Guid surveyId)
    {
        var statistics = await Data
            .FirstOrDefaultAsync(s => s.SurveyId == surveyId);

        await DeleteAsync(statistics);
    }

    public async Task<bool> IsStatisticsExists(Guid surveyId)
    {
        var isExists = await Data.AnyAsync(s => s.SurveyId == surveyId);

        return isExists;
    }


    private IQueryable<SurveyStatistics> GetStatisticQuery()
    {
        return Data
            .Include(e => e.Personalities)
            .Include(e => e.QuestionStatistics)
            .ThenInclude(e => (e as CheckboxQuestionStatistics)!.OptionsStatistics)
            .Include(e => e.QuestionStatistics)
            .ThenInclude(e => (e as RadioQuestionStatistics)!.OptionsStatistics);
    }
}