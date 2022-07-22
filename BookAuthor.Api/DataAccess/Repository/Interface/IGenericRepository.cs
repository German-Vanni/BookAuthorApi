using BookAuthor.Api.Model.Paging;
using System.Linq.Expressions;
using X.PagedList;

namespace BookAuthor.Api.DataAccess.Repository.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> Get(
                Expression<Func<T, bool>> expression = null,
                List<string> includes = null
            );
        Task<IEnumerable<T>> GetMany(
                Expression<Func<T, bool>> expression = null,
                List<string> includes = null
            );
        Task<IPagedList<T>> GetAll(
                RequestParameters requestParameters,
                List<string> includes = null
            );

        Task Add(T entity);
        Task AddRange(IEnumerable<T> entities);
        void Delete(int id);
        void DeleteRange(IEnumerable<T> entities);
        void Update(T entity);
    }
}
