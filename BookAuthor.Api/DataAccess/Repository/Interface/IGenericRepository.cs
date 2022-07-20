using System.Linq.Expressions;

namespace BookAuthor.Api.DataAccess.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(
                Expression<Func<T, bool>> expression = null,
                List<string> includes = null
            );
        Task<IEnumerable<T>> GetAll(
                Expression<Func<T, bool>> expression = null,
                List<string> includes = null
            );
        
        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
