using Core.Entity;
using Core.Interfaces;
using MongoDB.Driver;

namespace DocumentService.Infrastructure.Repository;

public class DocumentRepository : IDocumentRepository
{
    private readonly IMongoCollection<Document> _documents;

    public DocumentRepository(IMongoService mongoService)
    {
        _documents = mongoService.Documents;
    }

    public async Task CreateAsync(Document document)
    {
        await _documents.InsertOneAsync(document);
    }

    public Task DeleteAsync(string id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Document>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Document> GetByIdAsync(string id)
    {
        throw new NotImplementedException();
    }
}