using CustomerService.Data.Mongo;
using CustomerService.Entity.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace CustomerService.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IMongoService _mongo;

    public CustomersController(IMongoService mongo)
    {
        _mongo = mongo;
    }

    
}