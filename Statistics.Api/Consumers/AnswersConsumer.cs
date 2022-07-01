using AutoMapper;
using MassTransit;
using Statistics.Models.Answers;
using Statistics.Services.Abstracts;
using SurveyMe.AnswersApi.Models.Queue;
using SurveyMe.Common.Exceptions;

namespace Statistics.Api.Consumers;

public sealed class AnswersConsumer : IConsumer<SurveyAnswerQueue>
{
    private readonly ILogger _logger;

    private readonly IMapper _mapper;

    private readonly IStatisticsService _statisticsService;
    
    
    public AnswersConsumer(ILogger logger, IMapper mapper, IStatisticsService statisticsService)
    {
        _logger = logger;
        _mapper = mapper;
        _statisticsService = statisticsService;
    }
    
    
    public async Task Consume(ConsumeContext<SurveyAnswerQueue> context)
    {
        var answerQueue = context.Message;
        
        if (answerQueue == null)
        {
            throw new BadRequestException("Queue message is empty");
        }
        
        _logger.LogInformation("AnswerApi sent model");

        var answer = _mapper.Map<SurveyAnswer>(answerQueue);

        await _statisticsService.UpdateStatisticsAsync(answer);
    }
}