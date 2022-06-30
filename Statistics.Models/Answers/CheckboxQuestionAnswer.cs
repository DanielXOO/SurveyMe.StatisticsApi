namespace Statistics.Models.Answers;

public class CheckboxQuestionAnswer : BaseQuestionAnswer
{
    public ICollection<OptionQuestionAnswer> Options { get; set; }
}