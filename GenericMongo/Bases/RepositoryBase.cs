using System.Linq.Expressions;
using GenericMongo.Interfaces;
using MongoDB.Driver;

namespace GenericMongo.Bases;

public class RepositoryBase<T> : IRepository<T> where T:BaseEntity
{

    private readonly IMongoCollection<T> _collection;
    public RepositoryBase(IMongoService<T> mongoService)
    {
        _collection = mongoService.Collection;
    }
    public virtual IEnumerable<T> Get(Expression<Func<T, bool>> predicate)
    {
        return _collection.Find(predicate).ToList();
    }
    public virtual IEnumerable<T> Get()
    {
        return _collection.Find(i=>true).ToList();
    }

    public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
    {
        return await _collection.Find(predicate).FirstOrDefaultAsync();
    }

    public virtual async Task<T> GetByIdAsync(string id)
    {
        return await _collection.Find(i=>i.Id == id).FirstOrDefaultAsync();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _collection.InsertOneAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _collection.InsertManyAsync(entities);
    }
    

    public  virtual async Task<T> UpdateAsync(T entity)
    {
        return await _collection.FindOneAndReplaceAsync(e=>e.Id == entity.Id,entity);
    }

    public virtual async Task<T> DeleteAsync(string id)
    {
        return await _collection.FindOneAndDeleteAsync(e => e.Id == id);
    }
}