﻿using System.Text.Json;

namespace CustomerService.Common.Models;

public class ErrorDetails
{
    public string Message { get; set; }
    public int StatusCode { get; set; }

    public ErrorDetails(string message, int code)
    {
        Message = message;
        StatusCode = code;
    }

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}