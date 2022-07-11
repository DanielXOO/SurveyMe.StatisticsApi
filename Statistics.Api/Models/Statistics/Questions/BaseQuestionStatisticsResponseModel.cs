using Statistics.Models.Answers;

namespace Statistics.Api.Models.Statistics.Questions;

/// <summary>
/// Basic question statistics model
/// </summary>
public abstract class BaseQuestionStatisticsResponseModel
{
    /// <summary>
    /// <value>Question type</value>
    /// </summary>
    public QuestionType QuestionType { get; set; }
    
    /// <summary>
    /// <value>Question statistics id</value>
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// <value>Question answers count</value>
    /// </summary>
    public int AnswersCount { get; set; }

    /// <summary>
    /// <value>Question id</value>
    /// </summary>
    public Guid QuestionId { get; set; }
}