using AutoMapper;
using MassTransit;
using Statistics.Models.Surveys;
using Statistics.Services.Abstracts;
using SurveyMe.Common.Exceptions;
using SurveyMe.QueueModels;
using ILogger = SurveyMe.Common.Logging.Abstracts.ILogger;

namespace Statistics.Api.Consumers;

public sealed class SurveysConsumer : IConsumer<SurveyQueueModel>
{
    private readonly ILogger _logger;

    private readonly IMapper _mapper;

    private readonly IStatisticsService _statisticsService;


    public SurveysConsumer(ILogger logger, IMapper mapper, IStatisticsService statisticsService)
    {
        _logger = logger;
        _mapper = mapper;
        _statisticsService = statisticsService;
    }

    
    public async Task Consume(ConsumeContext<SurveyQueueModel> context)
    {
        var survey = context.Message;

        if (survey == null)
        {
            throw new BadRequestException("Queue message is empty");
        }
        
        _logger.LogInformation($"SurveyApi sent action {survey.EventType}");
        
        await HandleEventAsync(survey);
    }


    private async Task HandleEventAsync(SurveyQueueModel surveyQueue)
    {
        var survey = _mapper.Map<Survey>(surveyQueue);
        
        switch (surveyQueue.EventType)
        {
            case EventType.Create:
                await _statisticsService.CreateStatisticsAsync(survey);
                break;
            case EventType.Update:
                
                break;
            case EventType.Delete:
                await _statisticsService.DeleteStatisticsAsync(survey);
                break;
            default:
                throw new 
                    ArgumentOutOfRangeException(nameof(surveyQueue), surveyQueue.EventType, "No such event");
        }
    }
}