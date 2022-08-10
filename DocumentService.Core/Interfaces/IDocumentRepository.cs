using Core.Entity;

namespace Core.Interfaces;

public interface IDocumentRepository
{
    Task CreateAsync(Document document);
    Task DeleteAsync(string id);
    Task<IEnumerable<Document>> GetAllAsync();
    Task<Document> GetByIdAsync(string id);
}