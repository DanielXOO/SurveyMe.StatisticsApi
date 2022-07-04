using Refit;
using SurveyMe.PersonsApi.Models.Response.Personality;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;

namespace Statistics.Data.Refit;

public interface IPersonsApi
{
    [Get("/api/persons/{id}")]
    Task<PersonalityResponseModel> GetPersonalityAsync(Guid id, SurveyOptionsGetRequestModel surveyOptions);
}