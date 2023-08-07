using AutoMapper;
using Server.Controllers.Resources;
using Server.Core.Models;
using Server.Extensions;

namespace Server.Mapping;

public class AppMapping : Profile
{
    public AppMapping()
    {
        CreateMap<LoginResource, User>();
        CreateMap<RegisterResource, User>();

        CreateMap<CreateSubjectResource, Subject>();
        CreateMap<UpdateSubjectResource, Subject>();
        CreateMap<Subject, GetSingleSubjectResource>();
        CreateMap<Subject, GetSubjectsResource>();
        CreateMap<AuthResult, AuthResultResource>();
        CreateMap<StudyWeek, StudyWeekResource>();

        CreateMap(typeof(ListResponse<>), typeof(ListResponseResource<>));
    }
}