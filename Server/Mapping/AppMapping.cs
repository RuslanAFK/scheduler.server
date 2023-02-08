using AutoMapper;
using Server.Controllers.Resources;
using Server.Core.Models;

namespace Server.Mapping;

public class AppMapping : Profile
{
    public AppMapping()
    {
        CreateMap<LoginResource, User>();
        CreateMap<RegisterResource, User>();

        CreateMap<CreateSubjectResource, Subject>();
        CreateMap<UpdateSubjectResource, Subject>();
        CreateMap<Subject, GetSubjectResource>();

        CreateMap(typeof(ListResponse<>), typeof(ListResponseResource<>));
    }
}