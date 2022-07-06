using Statistics.Models.Statistics.Surveys;

namespace Statistics.Models.Personalities;

public sealed class PersonalityInfo
{
    public Guid Id { get; set; }

    public Guid PersonalityId { get; set; }
    
    public Guid SurveyStatisticsId { get; set; }
    
    public SurveyStatistics SurveyStatistics { get; set; }
}