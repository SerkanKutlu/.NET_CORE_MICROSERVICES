﻿namespace UserService.Core.Models;

public class Token
{
    public string AccessToken { get; set; }
    public DateTime Expiration { get; set; }
}