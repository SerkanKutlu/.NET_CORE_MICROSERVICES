using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
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
        token.Expiration = DateTime.Now.AddMinutes(5);
        var jwtToken = new JwtSecurityToken(
            _configuration["Token:Issuer"],
            _configuration["Token:Audience"],
            expires: token.Expiration,
            notBefore: DateTime.Now,
            signingCredentials: signingCredentials);
        var tokenHandler = new JwtSecurityTokenHandler();
        token.AccessToken = tokenHandler.WriteToken(jwtToken);
        return token;
    }
}