namespace Statistics.Api.Models.Statistics.Options;

public sealed class RadioOptionAnswerStatisticsResponseModel
{
    public Guid Id { get; set; }

    public Guid OptionId { get; set; }

    public int AnswersCount { get; set; }
    
}