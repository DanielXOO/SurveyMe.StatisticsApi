using Statistics.Api.Models.Statistics.Options;

namespace Statistics.Api.Models.Statistics.Questions;

public class CheckboxQuestionStatisticsResponseModel : BaseQuestionStatisticsResponseModel
{
    public ICollection<CheckboxOptionAnswerStatisticsResponseModel> OptionsStatistics { get; set; }
}