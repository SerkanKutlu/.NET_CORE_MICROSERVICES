using CustomerService.Application.Interfaces;

namespace CustomerService.Application.Handlers;

public abstract class AbstractDeleteHandler : IHandler
{
    private IHandler _nextHandler;
    public IHandler SetNext(IHandler handler)
    {
        _nextHandler = handler;
        return handler;
    }
        
    public virtual async Task<object> Handle(object request)
    {
        if (_nextHandler != null)
        {
            return await _nextHandler.Handle(request);
        }
        return null;
    }
    
}