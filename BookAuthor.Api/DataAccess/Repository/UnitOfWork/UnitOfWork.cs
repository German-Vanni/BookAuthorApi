using BookAuthor.Api.DataAccess.Repository.Interface;
using BookAuthor.Api.Model;

namespace BookAuthor.Api.DataAccess.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private AppDbContext _dbContext;
        //private IGenericRepository<Book> _books;
        //private IGenericRepository<Author> _author;

        public IGenericRepository<Book> Books { get; private set; } 
        public IGenericRepository<Author> Authors { get; private set; }
        public IGenericRepository<BookScore> Ratings { get; private set; }

        public UnitOfWork(AppDbContext dbContext)
        {
            _dbContext = dbContext;
            Books = new GenericRepository<Book>(dbContext);
            Authors= new GenericRepository<Author>(dbContext);
            Ratings = new GenericRepository<BookScore>(dbContext);
        }

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
