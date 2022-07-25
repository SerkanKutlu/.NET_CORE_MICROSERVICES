namespace UserService.Data.Exceptions;

public class NotFoundException<T>:CustomExceptionBase where T:class
{
    public NotFoundException() : base(new ErrorDetails($"{typeof(T).Name} is not found",404))
    {
        var x = typeof(T);
        Console.WriteLine(x);
        ;
    }
    public NotFoundException(string id) : base(new ErrorDetails($"{typeof(T).Name} with {id} is not found",404))
    {
    }
}