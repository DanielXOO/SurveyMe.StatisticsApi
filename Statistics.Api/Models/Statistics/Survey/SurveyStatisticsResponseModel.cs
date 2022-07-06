using Statistics.Api.Models.Statistics.Questions;
using Statistics.Models.Personalities;

namespace Statistics.Api.Models.Statistics.Survey;

public class SurveyStatisticsResponseModel
{
    public Guid Id { get; set; }
    
    public Guid SurveyId { get; set; }
    
    public int AnswersCount { get; set; }
    
    public int AverageAge { get; set; }

    public Dictionary<Gender, int>? GenderStatistics { get; set; }
    
    public ICollection<BaseQuestionStatisticsResponseModel> QuestionStatistics { get; set; }
}