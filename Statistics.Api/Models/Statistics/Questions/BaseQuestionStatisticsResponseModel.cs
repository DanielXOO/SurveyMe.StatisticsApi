using Statistics.Models.Answers;

namespace Statistics.Api.Models.Statistics.Questions;

public abstract class BaseQuestionStatisticsResponseModel
{
    public QuestionType QuestionType { get; set; }
    
    public Guid Id { get; set; }

    public int AnswersCount { get; set; }

    public Guid QuestionId { get; set; }
}