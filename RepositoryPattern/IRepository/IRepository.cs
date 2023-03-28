using System.Linq.Expressions;

namespace IotSupplyStore.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<T> GetFirstOrDefaultAsync(Expression<Func<T, bool>> filter, bool tracked = true);
        T GetFirstOrDefault(Expression<Func<T, bool>> filter, bool tracked = true);
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        Task Add(T entity);
        Task AddRange (IEnumerable<T> entities);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
