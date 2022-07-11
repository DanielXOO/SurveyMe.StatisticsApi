using Refit;
using SurveyMe.PersonsApi.Models.Request.Personality;
using SurveyMe.PersonsApi.Models.Response.Personality;

namespace Statistics.Data.Refit;

public interface IPersonsApi
{
    [Get("/persons-api/persons/{id}")]
    Task<PersonalityResponseModel> GetPersonalityAsync(Guid id, [Query] PersonalityGetRequestModel personality);
}