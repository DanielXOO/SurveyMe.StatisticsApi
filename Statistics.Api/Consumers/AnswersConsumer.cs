using AutoMapper;
using MassTransit;
using Statistics.Models.Answers;
using SurveyMe.AnswersApi.Models.Queue;
using SurveyMe.Common.Exceptions;

namespace Statistics.Api.Consumers;

public class AnswersConsumer : IConsumer<SurveyAnswerQueue>
{
    private readonly ILogger _logger;

    private readonly IMapper _mapper;
    
    
    public AnswersConsumer(ILogger logger, IMapper mapper)
    {
        _logger = logger;
        _mapper = mapper;
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

        //TODO: call BLL
    }
}