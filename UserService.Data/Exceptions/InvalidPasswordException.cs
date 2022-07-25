namespace UserService.Data.Exceptions;

public class InvalidPasswordException : CustomExceptionBase
{
    public InvalidPasswordException() : base(new ErrorDetails("Wrong user credentials",401))
    {
    }
}