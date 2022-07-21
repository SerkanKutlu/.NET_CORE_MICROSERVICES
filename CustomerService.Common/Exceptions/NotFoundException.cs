using CustomerService.Common.Models;

namespace CustomerService.Common.Exceptions;

public class NotFoundException<T>:CustomExceptionBase
{
    public NotFoundException(ErrorDetails errorDetails) : base(new ErrorDetails($"{nameof(T)} is not found",404))
    {
    }
}