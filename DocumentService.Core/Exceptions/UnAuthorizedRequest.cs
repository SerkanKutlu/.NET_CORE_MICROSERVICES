namespace Core.Exceptions
{
    public class UnAuthorizedRequest:CustomExceptionBase
    {
        public UnAuthorizedRequest(string message) : base(new ErrorDetails(message,403))
        {
        }
    }
}