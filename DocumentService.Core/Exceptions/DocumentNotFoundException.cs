namespace Core.Exceptions;

public class DocumentNotFoundException:CustomExceptionBase
{
    public DocumentNotFoundException() : base(new ErrorDetails("Document is not found",404))
    {
    }
}