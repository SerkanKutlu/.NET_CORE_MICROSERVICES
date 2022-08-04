using CustomerService.Application.Dto;
using CustomerService.Application.Interfaces;
using CustomerService.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CustomerServiceClean.API.Controllers;

[ApiController]
[Route("api/[controller]")]

public class CustomersController : ControllerBase
{
    private readonly ICustomerService _customerService;
    public CustomersController(ICustomerService customerService)
    {
        _customerService = customerService;
    }
    
    #region Get Requests
    
    /// <summary>
    /// List all customers
    /// </summary>
    /// <param name="requestParameters">Paging params if needed</param>
    /// <response code="200">Successful Response</response>
    /// <response code="500">Server Error</response>
    [HttpGet]
    //[Authorize]
    public async Task<IActionResult> GetAll([FromQuery] RequestParameters requestParameters)
    {
        var customers = await _customerService.GetAllCustomers(requestParameters,HttpContext);
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
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetById(string id)
    {
        var customer =await _customerService.GetById(id);
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
        await _customerService.ValidateCustomer(id);
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
        var customerId = await _customerService.CreateCustomer(customerForCreation,HttpContext);
        return Ok(customerId);
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
        var customer = await _customerService.UpdateCustomer(customerForUpdate);
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
        await _customerService.DeleteCustomer(id);
        return Ok();
    }
    #endregion

}