using Microsoft.EntityFrameworkCore;
using Statistics.Models.Answers;
using Statistics.Models.Statistics.Questions;
using Statistics.Models.Statistics.Surveys;

namespace Statistics.Data.EFCore;

public class StatisticsDbContext : DbContext
{
    public StatisticsDbContext(DbContextOptions<StatisticsDbContext> options) 
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BaseQuestionStatistics>(b =>
        {
            b.HasDiscriminator(e => e.QuestionType)
                .HasValue<TextQuestionStatistics>(QuestionType.Text)
                .HasValue<FileQuestionStatistics>(QuestionType.File)
                .HasValue<CheckboxQuestionStatistics>(QuestionType.Checkbox)
                .HasValue<RateQuestionStatistics>(QuestionType.Rate)
                .HasValue<ScaleQuestionStatistics>(QuestionType.Scale)
                .HasValue<RadioQuestionStatistics>(QuestionType.Radio);
        });

        modelBuilder.Entity<TextQuestionStatistics>();
        
        modelBuilder.Entity<FileQuestionStatistics>();
        
        modelBuilder.Entity<CheckboxQuestionStatistics>(b =>
        {
            b.HasMany(e => e.OptionsStatistics)
                .WithOne(e => e.CheckboxQuestionStatistics)
                .HasForeignKey(e => e.CheckboxQuestionStatisticsId);
        });
        
        modelBuilder.Entity<RateQuestionStatistics>();
        
        modelBuilder.Entity<ScaleQuestionStatistics>();
        
        modelBuilder.Entity<RadioQuestionStatistics>(b =>
        {
            b.HasMany(e => e.OptionsStatistics)
                .WithOne(e => e.CheckboxQuestionStatistics)
                .HasForeignKey(e => e.RadioQuestionStatisticsId);
        });

        modelBuilder.Entity<SurveyStatistics>(b =>
        {
            b.HasMany(e => e.Personalities)
                .WithOne(e => e.SurveyStatistics)
                .HasForeignKey(e => e.SurveyStatisticsId);
            
            b.HasMany(e => e.QuestionStatistics)
                .WithOne(e => e.SurveyStatistics)
                .HasForeignKey(e => e.SurveyStatisticsId);
        });
    }
}