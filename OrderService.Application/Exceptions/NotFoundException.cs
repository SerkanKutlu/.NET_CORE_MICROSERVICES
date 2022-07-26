using OrderService.Application.Models;

namespace OrderService.Application.Exceptions;

public class NotFoundException<T>:CustomExceptionBase
{
    public NotFoundException() : base(new ErrorDetails($"{typeof(T).Name} is not found",404))
    {
    }
    public NotFoundException(string id) : base(new ErrorDetails($"{typeof(T).Name} with {id} is not found",404))
    {
    }
}