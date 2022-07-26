using AutoMapper;
using UserService.Core.DTO;
using UserService.Core.Entity;

namespace UserService.Core.Mappings;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<UserForRegisterDto, User>();
    }
    
}