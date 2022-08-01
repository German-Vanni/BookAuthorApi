using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;

namespace BookAuthor.Api.Services.BookService
{
    public interface IBookService
    {
        Task<BookDto> GetBook(int id);
        Task<BookDto> CreateBook(BookDtoForCreation bookDto, bool Approved = false);
        Task UpdateBook(int bookId, BookDtoForUpdation bookDto);
        Task DeleteBook(int id);
        Task RateBook(RateBookDto rateBookDto, int BookId, ApiUser user);
        Task ApproveBook(int id);
        Task<List<BookDto>> GetAllBooks_Paged(int pageNumber, int pageSize);
        Task<List<BookDto>> GetUnapprovedBooks_Paged(int pageNumber, int pageSize);
    }
}
