using SurveyMe.AuthenticationApi.Models.Request;

namespace Statistics.Services.Abstracts;

public interface IAuthenticationService
{
    Task<string> GetTokenAsync(GetTokenRequestModel request);
}