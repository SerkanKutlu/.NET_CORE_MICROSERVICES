namespace CustomerService.Application.Interfaces;

public interface IHandler
{
    IHandler SetNext(IHandler handler);
    Task<object> Handle(object request);
}