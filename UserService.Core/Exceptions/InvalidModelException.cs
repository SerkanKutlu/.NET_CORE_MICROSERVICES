namespace UserService.Core.Exceptions;

public class InvalidModelException :CustomExceptionBase
{
    public InvalidModelException(ErrorDetails errorDetails) :  base(new ErrorDetails(message:"One or more validation failed",400))
    {
    }
    public InvalidModelException(string message) :  base(new ErrorDetails(message:message,400))
    {
    }
}