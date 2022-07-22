using OrderService.Common.Models;

namespace OrderService.Common.Exceptions;

public class InvalidModelException :CustomExceptionBase
{
    public InvalidModelException() :  base(new ErrorDetails(message:"One or more validation failed",400))
    {
    }
    public InvalidModelException(string message) :  base(new ErrorDetails(message:message,400))
    {
    }
}