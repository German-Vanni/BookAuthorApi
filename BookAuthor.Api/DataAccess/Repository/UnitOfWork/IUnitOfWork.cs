using BookAuthor.Api.DataAccess.Repository.Interface;
using BookAuthor.Api.Model;

namespace BookAuthor.Api.DataAccess.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Book> Books { get; }
        IGenericRepository<Author> Authors { get; }
        Task Save();
    }
}
