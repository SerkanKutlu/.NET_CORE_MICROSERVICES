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
    
}