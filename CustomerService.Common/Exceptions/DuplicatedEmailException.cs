using CustomerService.Common.Models;

namespace CustomerService.Common.Exceptions;

public class DuplicatedEmailException:CustomExceptionBase
{
    public DuplicatedEmailException():base(new ErrorDetails("This email is in use",400))
    {
        
    }
}