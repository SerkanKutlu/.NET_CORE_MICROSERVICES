namespace UserService.Core.Exceptions;

public class InvalidTokenException:CustomExceptionBase
{
    public InvalidTokenException() : base(new ErrorDetails("Log-in Again",401))
    {
    }
}