using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderService.Common.Exceptions;
using OrderService.Entity.Models;
using OrderService.Repository.Repository.Interfaces;

namespace OrderService.Core.ActionFilters;

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