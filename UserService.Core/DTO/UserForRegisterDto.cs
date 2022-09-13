using UserService.Core.Entity;

namespace UserService.Core.DTO;

public class UserForRegisterDto
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public User ToUser()
    {

        return new User
        {
            Id = Guid.NewGuid().ToString(),
            Name = Name,
            Surname = Surname,
            Email = Email,
            Password = Password,
            Role = "User",
            PasswordChanged = false
        };

    }
}

