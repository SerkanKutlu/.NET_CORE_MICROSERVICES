namespace Core.Exceptions
{
    public class DocumentNotSelectedException:CustomExceptionBase
    {
        public DocumentNotSelectedException( ) : base(new ErrorDetails("Select a file to upload", 400))
        {
        }
    }
}