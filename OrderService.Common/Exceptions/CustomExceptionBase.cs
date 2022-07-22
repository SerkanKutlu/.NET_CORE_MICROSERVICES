using System.Text.Json;
using OrderService.Common.Models;

namespace OrderService.Common.Exceptions;

public abstract class CustomExceptionBase:Exception
{
    public ErrorDetails ErrorDetails { get; set; }

    protected CustomExceptionBase(ErrorDetails errorDetails)
    {
        ErrorDetails = errorDetails;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(ErrorDetails);
    }
}