using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using UserService.API.Models;

namespace UserService.API;

public class TokenHandler : ITokenHandler
{
    private readonly ConfigurationManager _configuration;

    public TokenHandler(ConfigurationManager configuration)
    {
        _configuration = configuration;
    }


    public Token CreateToken()
    {
        var token = new Token();
        var symmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));
        var signingCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);
        token.Expiration = DateTime.UtcNow.AddMinutes(10);
        var claimList = new List<Claim>
        {
            new Claim("name", "serkan"),
            new Claim("surname", "kutlu"),
            new Claim("role", "Admin"),
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