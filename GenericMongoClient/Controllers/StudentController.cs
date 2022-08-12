using Microsoft.AspNetCore.Mvc;

namespace GenericMongoClient.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : Controller
{

    private readonly IStudentRepository _repo;

    public StudentController(IStudentRepository repo)
    {
        _repo = repo;
    }

   
    
}