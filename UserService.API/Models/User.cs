namespace UserService.API.Models;

public class User
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string RefreshToken { get; set; }
    public DateTime? RefreshTokenEndDate { get; set; }
}