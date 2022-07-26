using AutoMapper;
using OrderService.Application.DTO;
using OrderService.Domain.Entities;

namespace OrderService.Application.Mappings;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
            
        CreateMap<OrderForCreationDto, Order>().
            ForMember(dest=>dest.CreatedAt,operation=>operation
                .MapFrom(source=>DateTime.Now.ToUniversalTime()))
            .ForMember(dest=>dest.Quantity,operation=>operation.
                MapFrom(source=>source.ProductIds.Count));
        CreateMap<OrderForUpdateDto, Order>().
            ForMember(dest => dest.UpdatedAt, operation => operation
                .MapFrom(source => DateTime.Now.ToUniversalTime()))
            .ForMember(dest=>dest.Quantity,operation=>operation.
                MapFrom(source=>source.ProductIds.Count));
        CreateMap<ProductForCreationDto, Product>();
        CreateMap<ProductForUpdateDto, Product>();
    }
}
