using Statistics.Api.Models.Statistics.Options;

namespace Statistics.Api.Models.Statistics.Questions;

public class RadioQuestionStatisticsResponseModel : BaseQuestionStatisticsResponseModel
{
    public ICollection<RadioOptionAnswerStatisticsResponseModel> OptionsStatistics { get; set; }
}