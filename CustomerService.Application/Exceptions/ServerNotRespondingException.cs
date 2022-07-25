using CustomerService.Application.Models;

namespace CustomerService.Application.Exceptions;

public class ServerNotRespondingException : CustomExceptionBase
{
    public ServerNotRespondingException() : base(new ErrorDetails("Error Occured, try again later", 500))
    {
    }
}