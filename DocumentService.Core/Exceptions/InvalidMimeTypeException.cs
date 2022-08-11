namespace Core.Exceptions
{
    public class InvalidMimeTypeException:CustomExceptionBase
    {
        public InvalidMimeTypeException() : base(new ErrorDetails("Unsupported mime type.",400))
        {
        }
        public InvalidMimeTypeException(string mimeType) : base(new ErrorDetails($"Unsupported mime type:  {mimeType}",400))
        {
        }
    }
}