using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using UserService.Core.Entity;
using UserService.Core.Interfaces;
using UserService.Core.Models;

namespace UserService.Infrastructure;

public class TokenHandler : ITokenHandler
{
    private readonly IConfiguration _configuration;

    public TokenHandler(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public Token CreateToken(User user)
    {
        var token = new Token();
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        token.Expiration = DateTime.UtcNow.AddMinutes(10);
        token.UserId = user.Id;
        var claimList = new List<Claim>
        {
            new ("Role", user.Role),
            new ("UserId", user.Id),
        };
        var jwtToken = new JwtSecurityToken(
            claims:claimList,
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        token.AccessToken = tokenHandler.WriteToken(jwtToken);
        token.IsValid = true;
        return token;
    }
}