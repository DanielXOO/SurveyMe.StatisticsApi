using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Statistics.Api.Models.Statistics.Survey;
using Statistics.Services.Abstracts;

namespace Statistics.Api.Controllers;

[ApiController]
[Route("/api/surveys")]
public class StatisticsController : Controller
{
    private readonly IStatisticsService _statisticsService;

    private readonly IMapper _mapper;
    
    
    public StatisticsController(IStatisticsService statisticsService, IMapper mapper)
    {
        _statisticsService = statisticsService;
        _mapper = mapper;
    }


    [HttpGet("{surveyId:guid}/[controller]")]
    public async Task<IActionResult> GetStatistics(Guid surveyId)
    {
        var statistics = await _statisticsService.GetStatisticsBySurveyId(surveyId);

        var statisticsResponse = _mapper.Map<SurveyStatisticsResponseModel>(statistics);
        
        return Ok(statisticsResponse);
    }
}