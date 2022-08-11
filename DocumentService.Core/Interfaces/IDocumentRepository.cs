using Core.Entity;
using Core.Model;

namespace Core.Interfaces
{
    public interface IDocumentRepository
    {
        Task CreateAsync(DocumentEntity document);
        Task DeleteAsync(string id);
        Task<List<PathReturnModel>> GetAllPathsAsync();
        Task<PathReturnModel> GetPathByIdAsync(string id);
        Task<List<DocumentEntity>> GetEntities();
        Task<List<DocumentEntity>> GetEntities(string userId);
    }
}