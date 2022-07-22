using OrderService.Common.Models;

namespace OrderService.Common.Exceptions;

public class NotFoundException<T>:CustomExceptionBase
{
    public NotFoundException() : base(new ErrorDetails($"{nameof(T)} is not found",404))
    {
    }
    public NotFoundException(string id) : base(new ErrorDetails($"{nameof(T)} with {id} is not found",404))
    {
    }
}