using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Statistics.Api.Models.Statistics.Survey;
using Statistics.Services.Abstracts;
using SurveyMe.Error.Models.Response;

namespace Statistics.Api.Controllers;

/// <summary>
/// Controller for getting survey statistics by id
/// </summary>
[ApiController]
[Route("/api/surveys")]
public class StatisticsController : Controller
{
    private readonly IStatisticsService _statisticsService;

    private readonly IMapper _mapper;
    
    /// <summary>
    /// Controllers constructor
    /// </summary>
    /// <param name="statisticsService">Statistics service instance</param>
    /// <param name="mapper">Automapper instance</param>
    public StatisticsController(IStatisticsService statisticsService, IMapper mapper)
    {
        _statisticsService = statisticsService;
        _mapper = mapper;
    }


    /// <summary>
    /// Endpoint for getting statistics by survey id
    /// </summary>
    /// <param name="surveyId">Survey id</param>
    /// <returns>Statistics model</returns>
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SurveyStatisticsResponseModel))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(BaseErrorResponse))]
    [HttpGet("{surveyId:guid}/[controller]")]
    public async Task<IActionResult> GetStatistics(Guid surveyId)
    {
        var statistics = await _statisticsService.GetStatisticsBySurveyId(surveyId);

        var statisticsResponse = _mapper.Map<SurveyStatisticsResponseModel>(statistics);
        
        return Ok(statisticsResponse);
    }
}