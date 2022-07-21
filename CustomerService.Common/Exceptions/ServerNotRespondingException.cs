using CustomerService.Common.Models;

namespace CustomerService.Common.Exceptions;

public class ServerNotRespondingException : CustomExceptionBase
{
    public ServerNotRespondingException() : base(new ErrorDetails("Error Occured, try again later", 500))
    {
    }
}