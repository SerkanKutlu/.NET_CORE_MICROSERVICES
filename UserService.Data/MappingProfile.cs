using AutoMapper;
using UserService.Data.DTO;
using UserService.Data.Entity;

namespace UserService.Data;

public class MappingProfile : Profile
{

    public MappingProfile()
    {
        CreateMap<UserForRegisterDto, User>();
    }
    
}