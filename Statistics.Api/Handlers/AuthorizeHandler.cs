using System.Net.Http.Headers;
using Statistics.Services.Abstracts;
using SurveyMe.AuthenticationApi.Models.Request;
using SurveyMe.Common.Exceptions;

namespace Statistics.Api.Handlers;

public class AuthorizeHandler : DelegatingHandler
{
    private readonly IAuthenticationService _authenticationService;
    
    
    public AuthorizeHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var requestToken = new GetTokenRequestModel
        {
            client_id = "api",
            client_secret = "api_secret",
            grant_type = "client_credentials",
            scope = "ApisScope"
        };

        var token = await _authenticationService.GetTokenAsync(requestToken);

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        return await base.SendAsync(request, cancellationToken);
    }
}