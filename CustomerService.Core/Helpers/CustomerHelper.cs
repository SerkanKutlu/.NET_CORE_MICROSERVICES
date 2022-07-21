using System.Net;
using CustomerService.Common.Exceptions;
using CustomerService.Core.Handlers;
using CustomerService.Entity.Models;
using CustomerService.Repository.CustomerRepository;

namespace CustomerService.Core.Helpers;

public class CustomerHelper : ICustomerHelper
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IHttpRequest _httpRequest;
    public CustomerHelper(ICustomerRepository customerRepository, IHttpRequest httpRequest)
    {
        _customerRepository = customerRepository;
        _httpRequest = httpRequest;
    }

    public async Task SetCreatedAt(Customer customerForUpdate)
    {
        var oldCustomer = await _customerRepository.GetWithId(customerForUpdate.Id);
        customerForUpdate.CreatedAt = oldCustomer.CreatedAt;
    }

    public async Task DeleteRelatedOrders(string customerId)
    {
        //Client has a name because it has a retry policy.
        var result =await _httpRequest.DeleteOrders(customerId);
        if (result.StatusCode!=HttpStatusCode.NotFound && result.StatusCode!= HttpStatusCode.OK )
        {
            throw new ServerNotRespondingException();
        }
    }
    public async Task StartDeleteChain(string customerId)
    {
        var customerDeleteHandler = new CustomerDeleteHandler(_customerRepository);
        var orderDeleteHandler = new OrderDeleteHandler(this);
        orderDeleteHandler.SetNext(customerDeleteHandler);
        await orderDeleteHandler.Handle(customerId);
    }
}