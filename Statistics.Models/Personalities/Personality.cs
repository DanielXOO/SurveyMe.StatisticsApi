using Statistics.Models.Statistics;
using Statistics.Models.Statistics.Surveys;

namespace Statistics.Models.Personalities;

public sealed class Personality
{
    public Guid Id { get; set; }
    
    public Guid SurveyStatisticsId { get; set; }
    
    public SurveyStatistics SurveyStatistics { get; set; }
}