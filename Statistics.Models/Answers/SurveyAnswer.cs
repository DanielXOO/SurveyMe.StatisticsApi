namespace Statistics.Models.Answers;

public class SurveyAnswer
{
    public Guid SurveyId { get; set; }

    public Guid PersonalityId { get; set; }

    public List<BaseQuestionAnswer> QuestionAnswers { get; set; } 
}