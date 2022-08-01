using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Exceptions;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;

namespace BookAuthor.Api.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task ApproveBook(int id)
        {
            var book = await _unitOfWork.Books.Get(a => a.Id == id);
            if (book == null)
            {
                throw new EntityNotFoundException("Book does not exist");
            }

            book.Approved = true;

            _unitOfWork.Books.Update(book);
            await _unitOfWork.Save();
        }

        public async Task<BookDto> CreateBook(BookDtoForCreation bookDto, bool approved)
        {
            var book = _mapper.Map<Book>(bookDto);

            if (bookDto.AuthorIds is null || bookDto.AuthorIds.Count == 0)
            {
                throw new BadRequestException("A book  should have atleast one author (authorIds is empty");
            }

            book.AuthorBooks = new List<AuthorBook>();
            List<Author> authorsForResultMapping = new();
            foreach (int id in bookDto.AuthorIds)
            {
                var author = await _unitOfWork.Authors.Get(a => a.Id == id);
                if (author == null)
                {
                    throw new BadRequestException($"AuthorId {id} does not exist");
                }
                authorsForResultMapping.Add(author);

                book.AuthorBooks.Add(new AuthorBook { AuthorId = author.Id, Book = book });
            }

            //automatically approve if user is an admin
            book.Approved = approved;

            await _unitOfWork.Books.Add(book);
            foreach (var ab in book.AuthorBooks)
            {
                await _unitOfWork.AuthorBooks.Add(ab);
            }
            await _unitOfWork.Save();

            var bookResultDto = _mapper.Map<BookDto>(book);
            bookResultDto.Authors = new List<AuthorDtoForNesting>();
            foreach (var a in authorsForResultMapping)
            {
                var nestedAuthor = _mapper.Map<AuthorDtoForNesting>(a);
                bookResultDto.Authors.Add(nestedAuthor);
            }

            return bookResultDto;
        }

        public async Task DeleteBook(int id)
        {
            var book = await _unitOfWork.Books.Get(b => b.Id == id);

            if (book == null) throw new EntityNotFoundException();

            _unitOfWork.Books.Delete(book.Id);
            await _unitOfWork.Save();

            return;
        }

        public async Task<List<BookDto>> GetAllBooks_Paged(int pageNumber, int pageSize)
        {
            var books = await _unitOfWork.Books
                .GetPaged(
                    pageNumber, pageSize,
                    b => b.Approved,
                    includes: new List<string>
                    {
                            "Ratings"
                    });
            var bookDto_list = _mapper
                .Map<IEnumerable<Book>, IEnumerable<BookDto>>(books)
                .ToList();
            for (int i = 0; i < books.Count; i++)
            {
                double? ratingAverage = null;
                if (books[i].Ratings.Count > 0) ratingAverage = books[i].Ratings.Average(r => r.Score);
                bookDto_list[i].Rating = ratingAverage;
            }
            //as scores are between 1 and 5, if no ratings are found for this book
            //null is asigned to it

            return bookDto_list;
        }

        public async Task<BookDto> GetBook(int id)
        {
            var book = await _unitOfWork.Books.Get(b => b.Id == id, new List<string> { "AuthorBooks.Author", "Ratings" });
            if (book == null)
            {
                throw new EntityNotFoundException();
            }

            if (!book.Approved)
            {
                throw new ConflictEntityException("Book is pending approval");
            }

            var bookDto = _mapper.Map<BookDto>(book);

            double? ratingAverage = null;
            if (book.Ratings.Count > 0) ratingAverage = book.Ratings.Average(r => r.Score);
            bookDto.Rating = ratingAverage;

            return bookDto;
        }

        public async Task<List<BookDto>> GetUnapprovedBooks_Paged(int PageNumber, int PageSize)
        {
            var books = await _unitOfWork.Books
                .GetPaged(
                PageNumber, PageSize,
                b => b.Approved == false
                );
            var bookDto_list = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books).ToList();

            return (bookDto_list);
        }

        public async Task RateBook(RateBookDto rateBookDto, int BookId, ApiUser user)
        {

            var book = await _unitOfWork.Books
                .Get(b => b.Id == BookId, includes: new List<string> { "Ratings" });

            if (book == null)
            {
                throw new EntityNotFoundException("Book does not exist");
            }

            if (!book.Approved)
            {
                throw new ConflictEntityException("Book is pending approval");
            }

            BookScore bookScoreOfUser = await _unitOfWork.Ratings
                .Get(r => r.UserId == user.Id && r.BookId == book.Id);

            if (bookScoreOfUser != null)
            {
                bookScoreOfUser.Score = rateBookDto.Score;
                _unitOfWork.Ratings.Update(bookScoreOfUser);
                await _unitOfWork.Save();
                return;
            }

            bookScoreOfUser = new BookScore()
            {
                Score = rateBookDto.Score,
                BookId = book.Id,
                UserId = user.Id
            };

            await _unitOfWork.Ratings.Add(bookScoreOfUser);
            await _unitOfWork.Save();

            return;
        }

        public async Task UpdateBook(int bookId, BookDtoForUpdation bookDto)
        {

            var book = await _unitOfWork.Books.Get(b => b.Id == bookId);

            if (book == null)
            {
                throw new EntityNotFoundException("Book does not exist");
            }

            _mapper.Map(bookDto, book);

            _unitOfWork.Books.Update(book);
            await _unitOfWork.Save();

            return;
        }
    }
}
