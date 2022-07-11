using Refit;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;

namespace Statistics.Data.Refit;

public interface ISurveyPersonOptionsApi
{
    [Get("/survey-person-options-api/surveys/{surveyId}/surveyperson")]
    Task<SurveyOptionsResponseModel> GetSurveyOptionsAsync(Guid surveyId);
}