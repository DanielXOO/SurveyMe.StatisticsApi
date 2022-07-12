using Statistics.Data.Refit;
using Statistics.Services.Abstracts;
using SurveyMe.AuthenticationApi.Models.Request;
using SurveyMe.Common.Exceptions;

namespace Statistics.Services;

public class AuthenticationService : IAuthenticationService
{
    private readonly IAuthenticationApi _authenticationApi;
    
    
    public AuthenticationService(IAuthenticationApi authenticationApi)
    {
        _authenticationApi = authenticationApi;
    }
    
    
    public async Task<string> GetTokenAsync(GetTokenRequestModel request)
    {
        if (request == null)
        {
            throw new BadRequestException("Request model is empty");
        }
        
        var response = await _authenticationApi.GetTokenAsync(request);

        return response.access_token;
    }
}