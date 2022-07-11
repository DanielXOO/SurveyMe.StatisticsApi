using Statistics.Api.Models.Statistics.Questions;
using Statistics.Models.Personalities;

namespace Statistics.Api.Models.Statistics.Survey;

/// <summary>
/// Statistics model
/// </summary>
public class SurveyStatisticsResponseModel
{
    /// <summary>
    /// <value>Statistics id</value>
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// <value>Survey id</value>
    /// </summary>
    public Guid SurveyId { get; set; }
    
    /// <summary>
    /// <value>Survey answers count</value>
    /// </summary>
    public int AnswersCount { get; set; }
    
    /// <summary>
    /// <value>Average ages of users</value>
    /// </summary>
    public int AverageAge { get; set; }

    /// <summary>
    /// <value>Statistics per gender</value>
    /// </summary>
    public Dictionary<Gender, int>? GenderStatistics { get; set; }
    
    /// <summary>
    /// <value>Statistics per questiion</value>
    /// </summary>
    public ICollection<BaseQuestionStatisticsResponseModel> QuestionStatistics { get; set; }
}