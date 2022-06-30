namespace Statistics.Models.Surveys;

public sealed class Survey
{
    public Guid Id { get; set; }

    public ICollection<Question> Questions { get; set; }
}