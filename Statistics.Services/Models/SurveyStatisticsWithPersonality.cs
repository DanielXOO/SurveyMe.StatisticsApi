using Statistics.Models.Personalities;
using Statistics.Models.Statistics.Surveys;

namespace Statistics.Services.Models;

public class SurveyStatisticsWithPersonality
{
    public Dictionary<Gender, int>? GenderStatistics { get; set; }
    
    public int? AverageAge { get; set; }
    
    public SurveyStatistics SurveyStatistics { get; set; }
}