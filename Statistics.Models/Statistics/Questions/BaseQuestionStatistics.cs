using Statistics.Models.Answers;
using Statistics.Models.Statistics.Surveys;

namespace Statistics.Models.Statistics.Questions;

public abstract class BaseQuestionStatistics
{
    public Guid Id { get; set; }

    public QuestionType QuestionType { get; set; }

    public int AnswersCount { get; set; }

    public Guid QuestionId { get; set; }
    
    public SurveyStatistics SurveyStatistics { get; set; }
    
    public Guid SurveyStatisticsId { get; set; }
}