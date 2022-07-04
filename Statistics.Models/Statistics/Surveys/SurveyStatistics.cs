using Statistics.Models.Personalities;
using Statistics.Models.Statistics.Questions;

namespace Statistics.Models.Statistics.Surveys;

public class SurveyStatistics
{
    public Guid Id { get; set; }
    
    public Guid SurveyId { get; set; }
    
    public int AnswersCount { get; set; }

    public ICollection<PersonalityInfo> Personalities { get; set; }
    
    public ICollection<BaseQuestionStatistics> QuestionStatistics { get; set; }
}