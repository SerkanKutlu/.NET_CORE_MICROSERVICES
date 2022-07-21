using CustomerService.Repository.CustomerRepository;

namespace CustomerService.Core.Handlers;

public class CustomerDeleteHandler:AbstractDeleteHandler
{
    private readonly ICustomerRepository _customerRepository;


    public CustomerDeleteHandler(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }

    public override async Task<object> Handle(object request)
    {
        await _customerRepository.DeleteAsync((string)request);
        return await base.Handle(request);
    }
    
}