using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderService.Application.Interfaces;
using OrderService.Domain.Entities;

namespace OrderService.Application.ActionFilters;

public class ProductExistAttribute:IAsyncActionFilter
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;
        
    public ProductExistAttribute(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.ActionArguments.ContainsKey("newOrder"))
        {
            var order = _mapper.Map<Order>(context.ActionArguments["newOrder"]);
            foreach (var productId in order.ProductIds)
            {
                await _productRepository.GetWithId(productId);
            }
            await next();
        }
       
    }
}