using AutoMapper;
using Microsoft.AspNetCore.Mvc.Filters;
using OrderService.Common.Exceptions;
using OrderService.Core.Helpers;
using OrderService.Core.Model;
using OrderService.Entity.Models;

namespace OrderService.Core.ActionFilters;

public class CustomerExistAttribute:IAsyncActionFilter
{
    private readonly IOrderHelper _orderHelper;
    private readonly IMapper _mapper;
    public CustomerExistAttribute(IOrderHelper orderHelper, IMapper mapper)
    {
        _orderHelper = orderHelper;
        _mapper = mapper;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var customerId = context.ActionArguments["customerId"];
        if (customerId != null)
        {
            await _orderHelper.CheckCustomer((string)customerId);
            await next();
        }
        var order = _mapper.Map<Order>(context.ActionArguments["newOrder"]);
        if (order !=null)
        {
            customerId = order.CustomerId;
            await _orderHelper.CheckCustomer((string)customerId);
            await next();
        }
        throw new InvalidModelException("Customer  is not found");

    }
}