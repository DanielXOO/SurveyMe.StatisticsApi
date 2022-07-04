using AutoMapper;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Request.Options.Survey;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Personality;
using SurveyMe.SurveyPersonApi.Models.Response.Options.Survey;

namespace Statistics.Services.AutoMapper.Profiles;

public class PersonalityOptionsProfile : Profile
{
    public PersonalityOptionsProfile()
    {
        CreateMap<PersonalityOptionResponseModel, PersonalityOptionGetRequestModel>();
        
        CreateMap<SurveyOptionsResponseModel, SurveyOptionsGetRequestModel>();
    }
}