using AutoMapper;
using CustomerService.Common.DTO;
using CustomerService.Entity.Models;

namespace CustomerService.Common.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
            
        CreateMap<CustomerForCreationDto,Customer>().
            ForMember(dest=>dest.CreatedAt,operation=>operation
                .MapFrom(source=>DateTime.Now.ToUniversalTime())).
            ForMember(dest=>dest.UpdatedAt,operation=>operation
                .MapFrom(dest=>DateTime.Now.ToUniversalTime())).
            ForMember(dest=>dest.Email, operation=>operation
                .MapFrom(source=>source.Email.ToLower()));
        CreateMap<CustomerForUpdateDto, Customer>().ForMember(dest => dest.UpdatedAt, operation => operation
                .MapFrom(source => DateTime.Now.ToUniversalTime())).
            ForMember(dest=>dest.Email, operation=>operation
                .MapFrom(source=>source.Email.ToLower()));
    }
}