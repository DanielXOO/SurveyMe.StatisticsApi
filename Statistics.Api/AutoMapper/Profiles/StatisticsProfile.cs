using AutoMapper;
using Statistics.Api.Models.Statistics.Questions;
using Statistics.Api.Models.Statistics.Survey;
using Statistics.Models.Statistics.Questions;
using Statistics.Services.Models;

namespace Statistics.Api.AutoMapper.Profiles;

public class StatisticsProfile : Profile
{
    public StatisticsProfile()
    {
        CreateMap<SurveyStatisticsWithPersonality, SurveyStatisticsResponseModel>()
            .ForMember(dest => dest.Id,
                opt 
                    => opt.MapFrom(src => src.SurveyStatistics.Id))
            .ForMember(dest => dest.SurveyId,
                opt 
                    => opt.MapFrom(src => src.SurveyStatistics.SurveyId))
            .ForMember(dest => dest.QuestionStatistics,
                opt 
                    => opt.MapFrom(src => src.SurveyStatistics.QuestionStatistics))
            .ForMember(dest => dest.AnswersCount,
                opt 
                    => opt.MapFrom(src => src.SurveyStatistics.AnswersCount));
        
        CreateMap<BaseQuestionStatistics, BaseQuestionStatisticsResponseModel>()
            .Include<TextQuestionStatistics, TextQuestionStatisticsResponseModel>()
            .Include<FileQuestionStatistics, FileQuestionStatisticsResponseModel>()
            .Include<RateQuestionStatistics, RateQuestionStatisticsResponseModel>()
            .Include<ScaleQuestionStatistics, ScaleQuestionStatisticsResponseModel>()
            .Include<RadioQuestionStatistics, RadioQuestionStatisticsResponseModel>()
            .Include<CheckboxQuestionStatistics, CheckboxQuestionStatisticsResponseModel>();

        CreateMap<TextQuestionStatistics, TextQuestionStatisticsResponseModel>();
        CreateMap<FileQuestionStatistics, FileQuestionStatisticsResponseModel>();
        CreateMap<RateQuestionStatistics, RateQuestionStatisticsResponseModel>();
        CreateMap<ScaleQuestionStatistics, ScaleQuestionStatisticsResponseModel>();
        CreateMap<RadioQuestionStatistics, RadioQuestionStatisticsResponseModel>();
        CreateMap<CheckboxQuestionStatistics, CheckboxQuestionStatisticsResponseModel>();

        CreateMap<CheckboxQuestionStatistics, CheckboxQuestionStatisticsResponseModel>();
        CreateMap<RadioQuestionStatistics, RadioQuestionStatisticsResponseModel>();
    }
}