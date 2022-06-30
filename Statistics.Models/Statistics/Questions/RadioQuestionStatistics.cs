using Statistics.Models.Statistics.Options;

namespace Statistics.Models.Statistics.Questions;

public class RadioQuestionStatistics : BaseQuestionStatistics
{
    public ICollection<RadioOptionAnswerStatistics> OptionsStatistics { get; set; }
}