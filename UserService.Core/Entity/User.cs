using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace UserService.Core.Entity;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public bool PasswordChanged{get;set;}
    
}