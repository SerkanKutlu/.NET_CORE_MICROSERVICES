using System.Text.Json;
using System.Text.Json.Serialization;
using AutoMapper;
using CustomerService.Common.DTO;
using CustomerService.Common.Models;
using CustomerService.Core.Helpers;
using CustomerService.Entity.Models;
using CustomerService.Repository.CustomerRepository;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson.IO;

namespace CustomerService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly ILogger<CustomersController> _logger;
    private readonly IMapper _mapper;
    private readonly ICustomerRepository _customerRepository;
    private readonly ICustomerHelper _customerHelper;
    public CustomersController(ILogger<CustomersController> logger, ICustomerRepository customerRepository, IMapper mapper, ICustomerHelper customerHelper)
    {
        _logger = logger;
        _customerRepository = customerRepository;
        _mapper = mapper;
        _customerHelper = customerHelper;
    }

    #region Get Requests
    
    /// <summary>
    /// List all customers
    /// </summary>
    /// <param name="requestParameters">Paging params if needed</param>
    /// <response code="200">Successful Response</response>
    /// <response code="500">Server Error</response>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
    {
        var customers =await _customerRepository.GetAll(requestParameters);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(customers.MetaData));
        _logger.LogInformation("Getting all customers data's from database");
        return Ok(customers);
    }
    
    /// <summary>
    /// Fetch a customer with giving id
    /// </summary>
    ///<param name="id">Id of specific customer</param>
    /// <response code="200">Successful Response</response>
    /// <response code="404">Invalid Customer Id</response>
    /// <response code="400">Invalid Id Format Error</response>
    /// <response code="500">Server Error</response>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var customer = await _customerRepository.GetWithId(id);
        _logger.LogInformation($"Getting customer data with {id} from database");
        return Ok(customer);
    }
    
    
    /// <summary>
    /// Check the if customer exist with an id.
    /// </summary>
    /// <remarks>
    /// This endpoint can be used by less authorized users.
    /// </remarks>
    /// <response code="200">Successful Response</response>
    /// <response code="404">Invalid Customer Id</response>
    /// <response code="400">Invalid Id Format Error</response>
    /// <response code="500">Server Error</response>
    ///<param name="id">Id of specific customer</param>
    [HttpGet("validate/{id}")]
    public async Task<IActionResult> ValidateCustomer(string id)
    {
        await _customerRepository.Validate(id);
        _logger.LogInformation($"Validated customer with {id} from database");
        return Ok();
    }
    #endregion
    #region Post Requests

    /// <summary>
    /// Creating new customer by using body.
    /// </summary>
    /// <response code="201">Successful Response</response>
    /// <response code="400">Validation Failed or Invalid Request Body</response>
    /// <response code="500">Server Error</response>
    /// <param name="customerForCreation">All areas are required</param>
    [HttpPost]
    public async Task<IActionResult> CreateCustomer([FromBody] CustomerForCreationDto customerForCreation)
    {
        var customer = _mapper.Map<Customer>(customerForCreation);
        await _customerRepository.CreateAsync(customer);
        HttpContext.Response.Headers.Add("location",$"https://{Request.Headers["Host"]}/api/Customers/{customer.Id}");
        _logger.LogInformation($"New customer added with id {customer.Id}");
        return Ok(customer.Id);
    }
    #endregion
    #region Put Requests
    /// <summary>
    /// Updating a customer by using body.
    /// </summary>
    /// <response code="200">Successful Response</response>
    /// <response code="400">Validation Failed or Invalid Request Body</response>
    /// <response code="404">Invalid Customer Id</response>
    /// <response code="500">Server Error</response>
    /// <param name="customerForUpdate">All areas are required. Enter correct pattern and valid id.</param>
    [HttpPut]
    public async Task<IActionResult> UpdateCustomer([FromBody] CustomerForUpdateDto customerForUpdate)
    {
        var customer = _mapper.Map<Customer>(customerForUpdate);
        await _customerHelper.SetCreatedAt(customer);
        await _customerRepository.UpdateAsync(customer);
        _logger.LogInformation($"Customer with id {customer.Id} updated.");
        return Ok(customer);
    }
    #endregion
    
    #region Delete Requests
    /// <summary>
    /// Deleting a customer by using id of the customer.
    /// </summary>
    /// <param name="id">Enter a valid id of a customer</param>
    /// <response code="200">Successful Response</response>
    /// <response code="404">Invalid Customer Id</response>
    /// <response code="503">Error With Remote Server</response>
    /// <response code="500">Server Error</response>
    /// <response code="400">Invalid Id Format Error</response>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        await _customerHelper.StartDeleteChain(id);
        _logger.LogInformation($"Customer with id {id} deleted.");
        return Ok();
    }
    #endregion

}