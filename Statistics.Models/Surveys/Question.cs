using Statistics.Models.Answers;

namespace Statistics.Models.Surveys;

public sealed class Question
{
    public Guid Id { get; set; }
    
    public QuestionType Type { get; set; }
    
    public ICollection<QuestionOptions> QuestionsOptions { get; set; }
}