using AutoMapper;
using Statistics.Api.Models.Statistics.Options;
using Statistics.Models.Answers;
using Statistics.Models.Statistics.Options;
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
        CreateMap<QuestionQueueModel, Question>()
            .ForMember(dest => dest.QuestionsOptions,
                opt => opt.MapFrom(src 
                    => src.Options));
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

        CreateMap<CheckboxOptionAnswerStatistics, CheckboxOptionAnswerStatisticsResponseModel>();

        CreateMap<OptionQuestionAnswerQueue, OptionQuestionAnswer>();
    }
}