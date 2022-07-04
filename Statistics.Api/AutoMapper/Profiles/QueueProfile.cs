using AutoMapper;
using Statistics.Models.Answers;
using Statistics.Models.Surveys;
using SurveyMe.AnswersApi.Models.Queue;
using SurveyMe.QueueModels;

namespace Statistics.Api.AutoMapper.Profiles;

public class QueueProfile : Profile 
{
    public QueueProfile()
    {
        CreateMap<SurveyAnswerQueue, SurveyAnswer>();

        CreateMap<SurveyQueueModel, Survey>();
        CreateMap<QuestionQueueModel, Question>();
        CreateMap<OptionQueueModel, QuestionOptions>();
        
        CreateMap<BaseQuestionAnswerQueue, BaseQuestionAnswer>()
            .Include<TextQuestionAnswerQueue, TextQuestionAnswer>()
            .Include<FileQuestionAnswerQueue, FileQuestionAnswer>()
            .Include<RateQuestionAnswerQueue, RateQuestionAnswer>()
            .Include<ScaleQuestionAnswerQueue, ScaleQuestionAnswer>()
            .Include<RadioQuestionAnswerQueue, RadioQuestionAnswer>()
            .Include<CheckboxQuestionAnswerQueue, CheckboxQuestionAnswer>();

        CreateMap<TextQuestionAnswerQueue, TextQuestionAnswer>();
        CreateMap<FileQuestionAnswerQueue, FileQuestionAnswer>();
        CreateMap<RateQuestionAnswerQueue, RateQuestionAnswer>();
        CreateMap<ScaleQuestionAnswerQueue, ScaleQuestionAnswer>();
        CreateMap<RadioQuestionAnswerQueue, RadioQuestionAnswer>();
        CreateMap<CheckboxQuestionAnswerQueue, CheckboxQuestionAnswer>();
    }
}