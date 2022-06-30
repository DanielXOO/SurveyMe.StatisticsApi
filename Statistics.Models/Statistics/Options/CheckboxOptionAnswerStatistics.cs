using Statistics.Models.Statistics.Questions;

namespace Statistics.Models.Statistics.Options;

public sealed class CheckboxOptionAnswerStatistics
{
    public Guid Id { get; set; }

    public Guid OptionId { get; set; }

    public int AnswersCount { get; set; }
    
    public Guid CheckboxQuestionStatisticsId { get; set; }
    
    public CheckboxQuestionStatistics CheckboxQuestionStatistics { get; set; }
}