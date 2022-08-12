using Core.Entity;
using Core.Model;
using GenericMongo.Interfaces;

namespace Core.Interfaces
{
    public interface IDocumentRepository : IRepository<DocumentEntity>
    {
        Task<List<PathReturnModel>> GetAllPathsAsync();
        Task<PathReturnModel> GetPathByIdAsync(string id);
    }
}