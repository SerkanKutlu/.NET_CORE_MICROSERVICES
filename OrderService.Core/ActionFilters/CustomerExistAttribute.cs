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
        if (context.ActionArguments.ContainsKey("customerId"))
        {
            var customerId = context.ActionArguments["customerId"];
            await _orderHelper.CheckCustomer((string)customerId);
            await next();
        }

        else if (context.ActionArguments.ContainsKey("newOrder"))
        {
            var order = _mapper.Map<Order>(context.ActionArguments["newOrder"]);
            var customerId = order.CustomerId;
            await _orderHelper.CheckCustomer(customerId);
            await next();
        }
       
        

    }
}