namespace UserService.Core.DTO;

public class UserForPasswordUpdateDto
{
    public string Email { get; set; }
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
}