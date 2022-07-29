using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookAuthor.Api.Controllers
{
    [ApiController]
    public class BookController : ApiControllerBase<BookController>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookController(
            IWebHostEnvironment environment,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            ILogger<BookController> logger,
            UserManager<ApiUser> userManager) 
            : base(environment, logger, userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet(Name ="GetBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks([FromQuery] RequestParameters requestParameters)
        {
            try
            {
                var books = await _unitOfWork.Books
                    .GetPaged(
                    requestParameters,
                    b => b.Approved,
                    includes: new List<string>
                    {
                        "Ratings"
                    });
                var bookDto_list = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books).ToList();
                for(int i = 0; i<books.Count; i++)
                {
                    double? ratingAverage = null;
                    if (books[i].Ratings.Count > 0) ratingAverage = books[i].Ratings.Average(r => r.Score);
                    bookDto_list[i].Rating = ratingAverage;
                }
                //foreach (var b in bookDto_list) b.Rating = b.Rating == 0 ? null : b.Rating;
                //as scores are between 1 and 5, if no ratings are found and it will give the default value 0
                return Ok(bookDto_list);
            }
            catch(Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }

        [HttpGet("{id:int}", Name ="GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {
            try
            {
                var book = await _unitOfWork.Books.Get(b => b.Id == id, new List<string> { "AuthorBooks.Author", "Ratings"});
                if(book == null)
                {
                    return NotFound();
                }
                if (!book.Approved)
                {
                    return Conflict("Book is pending approval");
                }

                var bookDto = _mapper.Map<BookDto>(book);

                double? ratingAverage = null;
                if(book.Ratings.Count > 0) ratingAverage = book.Ratings.Average(r => r.Score);
                bookDto.Rating = ratingAverage;

                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost(Name = "CreateBook")]
        public async Task<IActionResult> CreateBook([FromBody] BookDtoForCreation bookDto)
        {
            ApiUser user;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                user = await GetClaimedUser();
                if(user is null)
                {
                    return ClaimedUserNotFound();
                }

                var book = _mapper.Map<Book>(bookDto);

                if(bookDto.AuthorIds is null || bookDto.AuthorIds.Count == 0)
                {
                    return BadRequest("A book  should have atleast one author (authorIds is empty");
                }

                book.AuthorBooks = new List<AuthorBook>();
                List<Author> authorsForResultMapping = new();
                foreach (int id in bookDto.AuthorIds)
                {
                    var author = await _unitOfWork.Authors.Get(a => a.Id == id);
                    if (author == null) return BadRequest($"AuthorId {id} does not exists");
                    authorsForResultMapping.Add(author);

                    book.AuthorBooks.Add(new AuthorBook { AuthorId = author.Id, Book = book });
                }

                book.Approved = await IsUserInRoles(user, "Admin");

                await _unitOfWork.Books.Add(book);
                foreach(var ab in book.AuthorBooks)
                {
                    await _unitOfWork.AuthorBooks.Add(ab);
                }
                await _unitOfWork.Save();

                var bookResultDto = _mapper.Map<BookDto>(book);
                bookResultDto.Authors = new List<AuthorDtoForNesting>();
                foreach(var a in authorsForResultMapping)
                {
                    var nestedAuthor = _mapper.Map<AuthorDtoForNesting>(a);
                    bookResultDto.Authors.Add(nestedAuthor);
                }
                
                return CreatedAtRoute("GetBook", new { id = book.Id }, bookResultDto);
            }
            catch(Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}", Name ="UpdateBook")]
        public async Task<IActionResult> UpdateBook(int? id, [FromBody] BookDtoForUpdation bookDto)
        {
            ApiUser user;

            if (id == null || id < 1) return BadRequest("Book id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var book = await _unitOfWork.Books.Get(b => b.Id == (int)id);
                if(book == null)
                {
                    return NotFound();
                }

                _mapper.Map(bookDto, book);
                _unitOfWork.Books.Update(book);
                await _unitOfWork.Save();

                return NoContent();
            }
            catch (Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }

        [Authorize(Roles="Admin")]
        [HttpDelete("{id:int}", Name="DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            ApiUser user;

            if (id == null || id < 1)
            {
                return BadRequest("Book id should be defined, and have a valid value");
            }
            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var book = await _unitOfWork.Books.Get(b => b.Id == id);

                if (book == null) return NotFound();

                _unitOfWork.Books.Delete(book.Id);
                await _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;
                //logging
                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }

        [Authorize]
        [HttpPost("rate/{id:int}", Name = "RateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RateBook(int? id,[FromBody] RateBookDto rateBookDto)
        {
            ApiUser user;

            if (id == null || id < 1)
            {
                return BadRequest("Book id should be defined, and have a valid value");
            }
            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var book = await _unitOfWork.Books.Get(b => b.Id == id, includes: new List<string> { "Ratings" });

                if (book == null) return NotFound();

                if (!book.Approved)
                {
                    return Conflict("Book is pending approval");
                }

                BookScore bookScoreOfUser = await _unitOfWork.Ratings
                    .Get(r => r.UserId == user.Id && r.BookId == book.Id);

                if (bookScoreOfUser != null)
                {
                    bookScoreOfUser.Score = rateBookDto.Score;
                    _unitOfWork.Ratings.Update(bookScoreOfUser);
                    await _unitOfWork.Save();
                    return Ok();
                }

                bookScoreOfUser = new BookScore()
                {
                    Score = rateBookDto.Score,
                    BookId = book.Id,
                    UserId = user.Id
                };

                await _unitOfWork.Ratings.Add(bookScoreOfUser);
                await _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("approve",Name = "GetUnapprovedBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnapprovedBooks([FromQuery] RequestParameters requestParameters)
        {
            ApiUser user;
            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var books = await _unitOfWork.Books
                    .GetPaged(
                    requestParameters,
                    b => b.Approved == false
                    );
                var bookDto_list = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books).ToList();
                
                return Ok(bookDto_list);
            }
            catch (Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("approve/{id:int}", Name = "ApproveBook")]
        public async Task<IActionResult> ApproveBook(int? id)
        {
            ApiUser user;

            if (id == null || id < 1) return BadRequest("Book id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var book = await _unitOfWork.Books.Get(a => a.Id == (int)id);
                if (book == null)
                {
                    return NotFound();
                }

                book.Approved = true;

                _unitOfWork.Books.Update(book);
                await _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                string message = ERROR_500_MSG;
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex, ex.Message);
                return StatusCode(500, message);
            }
        }
    }
}
