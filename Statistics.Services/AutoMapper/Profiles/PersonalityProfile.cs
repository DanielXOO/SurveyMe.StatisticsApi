using AutoMapper;
using Statistics.Models.Personalities;
using SurveyMe.PersonsApi.Models.Response.Personality;

namespace Statistics.Services.AutoMapper.Profiles;

public class PersonalityProfile : Profile
{
    public PersonalityProfile()
    {
        CreateMap<PersonalityResponseModel, PersonalityInfo>();
    }
}