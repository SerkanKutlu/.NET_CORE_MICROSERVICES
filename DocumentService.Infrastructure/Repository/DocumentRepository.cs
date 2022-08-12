using System.Linq.Expressions;
using Aspose.Words;
using Core.Entity;
using Core.Exceptions;
using Core.Interfaces;
using Core.Model;
using GenericMongo;
using GenericMongo.Bases;
using GenericMongo.Interfaces;
using MongoDB.Driver;

namespace DocumentService.Infrastructure.Repository;

public class DocumentRepository : RepositoryBase<DocumentEntity>, IDocumentRepository
{
    private readonly IMongoCollection<DocumentEntity> _documents;

    public DocumentRepository(IMongoService<DocumentEntity> mongoService) : base(mongoService)
    {
        _documents = mongoService.Collection;
    }
    
    public async Task<List<PathReturnModel>> GetAllPathsAsync()
    {
        var projection = Builders<DocumentEntity>
            .Projection
            .Include(d => d.Path)
            .Include(d => d.FileName)
            .Include(d => d.UserId);
        var documents = await _documents.Find(d => true)
            .Project<PathReturnModel>(projection).ToListAsync();
        if (!documents.Any())
            throw new DocumentNotFoundException();
        return documents;
    }


    public async Task<PathReturnModel> GetPathByIdAsync(string id)
    {
        var projection = Builders<DocumentEntity>
            .Projection
            .Include(d => d.Path)
            .Include(d => d.FileName)
            .Include(d => d.UserId);
        var document = await _documents.Find(d => d.Id == id)
            .Project<PathReturnModel>(projection).FirstOrDefaultAsync();
        if (document == null)
            throw new DocumentNotFoundException();
        return document;
    }
    
}