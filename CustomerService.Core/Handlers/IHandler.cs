namespace CustomerService.Core.Handlers;

public interface IHandler
{
    IHandler SetNext(IHandler handler);
    Task<object> Handle(object request);
}