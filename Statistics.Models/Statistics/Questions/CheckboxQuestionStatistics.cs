using Statistics.Models.Statistics.Options;

namespace Statistics.Models.Statistics.Questions;

public class CheckboxQuestionStatistics : BaseQuestionStatistics
{
    public ICollection<CheckboxOptionAnswerStatistics> OptionsStatistics { get; set; }
}