using CustomerService.Application.Models;

namespace CustomerService.Application.Exceptions;

public class DuplicatedEmailException:CustomExceptionBase
{
    public DuplicatedEmailException():base(new ErrorDetails("This email is in use",400))
    {
        
    }
}