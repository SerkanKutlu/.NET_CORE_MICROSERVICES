using GenericMongo.Interfaces;
using UserService.Core.Models;

namespace UserService.Core.Interfaces;

public interface ITokenRepository: IRepository<Token>
{
}