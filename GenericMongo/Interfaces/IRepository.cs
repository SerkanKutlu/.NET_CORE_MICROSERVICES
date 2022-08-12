using System.Linq.Expressions;

namespace GenericMongo.Interfaces;

public interface IRepository<T> where T:class
{
    IEnumerable<T> Get(Expression<Func<T, bool>> predicate);
    IEnumerable<T> Get();
    Task<T> GetAsync(Expression<Func<T, bool>> predicate);
    Task<T> GetByIdAsync(string id);
    Task AddAsync(T entity);
    Task AddRangeAsync(IEnumerable<T> entities);
    Task<T> UpdateAsync(T entity);
    Task<T> DeleteAsync(string id);
}