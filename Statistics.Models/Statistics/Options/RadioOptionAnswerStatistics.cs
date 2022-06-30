using Statistics.Models.Statistics.Questions;

namespace Statistics.Models.Statistics.Options;

public sealed class RadioOptionAnswerStatistics
{
    public Guid Id { get; set; }

    public Guid OptionId { get; set; }

    public int AnswersCount { get; set; }
    
    public Guid RadioQuestionStatisticsId { get; set; }
    
    public RadioQuestionStatistics CheckboxQuestionStatistics { get; set; }
}