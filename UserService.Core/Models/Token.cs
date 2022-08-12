using GenericMongo.Bases;

namespace UserService.Core.Models;

public class Token : BaseEntity
{
    public string UserId { get; set; }
    public string AccessToken { get; set; }
    
    public DateTime Expiration { get; set; }
    public bool IsValid { get; set; }
}