using Refit;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;

namespace Statistics.Data.Refit;

public interface ISurveyPersonOptionsApi
{
    [Get("/api/surveys/{surveyId}/surveyperson")]
    Task<SurveyOptionsResponseModel> GetSurveyOptionsAsync(Guid surveyId);
}