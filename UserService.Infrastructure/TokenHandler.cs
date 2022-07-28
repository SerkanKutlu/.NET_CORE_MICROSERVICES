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
        var claimList = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(ClaimTypes.Surname, user.Surname),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };
        var jwtToken = new JwtSecurityToken(
            _configuration["Token:Issuer"],
            _configuration["Token:Audience"],
            claims:claimList,
            expires: token.Expiration,
            notBefore: DateTime.UtcNow,
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        token.AccessToken = tokenHandler.WriteToken(jwtToken);
        return token;
    }
}