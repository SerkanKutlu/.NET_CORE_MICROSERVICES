using CustomerService.Application.Helpers;
using CustomerService.Application.Interfaces;

namespace CustomerService.Application.Handlers;

public class OrderDeleteHandler : AbstractDeleteHandler
{

    private readonly ICustomerHelper _customerHelper;

    public OrderDeleteHandler(ICustomerHelper customerHelper)
    {
        _customerHelper = customerHelper;
    }
        
    public override async Task<object> Handle(object request)
    {
        await _customerHelper.DeleteRelatedOrders((string)request);
        return await base.Handle(request);
    }
    
}