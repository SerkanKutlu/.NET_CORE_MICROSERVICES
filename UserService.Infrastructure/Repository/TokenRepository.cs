using GenericMongo.Bases;
using GenericMongo.Interfaces;
using MongoDB.Driver;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.Core.Models;

namespace UserService.Infrastructure.Repository;

public class TokenRepository : RepositoryBase<Token>,ITokenRepository
{
    private readonly IMongoCollection<Token> _tokens;
    public TokenRepository(IMongoService<Token> mongoService) : base(mongoService)
    {
        _tokens = mongoService.Collection;
    }
    
    public async Task ValidateToken(string id)
    {
        var token = await _tokens.Find(t => t.Id == id).FirstOrDefaultAsync();
        if (token == null)
        {
            throw new NotFoundException<Token>();
        }

        if (!token.IsValid)
        {
            throw new InvalidTokenException();
        }
    }

    public async Task SetTokenInvalidate(string userId)
    {
        var token = await _tokens.Find(t => t.UserId == userId).FirstOrDefaultAsync();
        if(token == null)
            return;
        token.IsValid = false;
        await UpdateAsync(token);
    }
}