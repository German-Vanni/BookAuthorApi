using BookAuthor.Api.DataAccess.Repository.Interface;
using BookAuthor.Api.Model.Paging;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using X.PagedList;

namespace BookAuthor.Api.DataAccess.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _appDbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
            _dbSet = _appDbContext.Set<T>();
        }


        public async Task Add(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task AddRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
            return;
        }

        public void Delete(int id)
        {
            var entity = _dbSet.Find(id);
            _dbSet.Remove(entity);
            return;
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression = null, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if(query != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                foreach (string i in includes)
                {
                    query = query.Include(i);
                }
            }
            

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> expression = null,  List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if(expression != null)
            {
                query = query.Where(expression);
            }
            if(includes != null)
            {
                foreach (string i in includes)
                {
                    query = query.Include(i);
                }
            }
            

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IPagedList<T>> GetAll(RequestParameters requestParameters, List<string> includes = null)
        {

            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                foreach (string i in includes)
                {
                    query = query.Include(i);
                }
            }


            return await query.AsNoTracking().ToPagedListAsync(requestParameters.PageNumber, requestParameters.PageSize);
        }

        public void Update(T entity)
        {
            _appDbContext.Attach(entity);
            _appDbContext.Entry(entity).State = EntityState.Modified;
            return;
        }
    }
}
