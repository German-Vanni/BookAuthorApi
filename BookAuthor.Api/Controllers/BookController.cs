using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookAuthor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public BookController(
            IWebHostEnvironment environment,
            IUnitOfWork unitOfWork,
            IMapper mapper, 
            ILogger<BookController> logger)
        {
            _environment = environment;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet(Name ="GetBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks([FromQuery] RequestParameters requestParameters)
        {
            try
            {
                var books = await _unitOfWork.Books.GetAll(requestParameters, new List<string>
                {
                    "Ratings"
                });
                var bookDto_list = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books);
                foreach (var b in bookDto_list) b.Rating = b.Rating == 0 ? null : b.Rating;
                //as scores are between 1 and 5, if no ratings are found and it will give the default value 0
                return Ok(bookDto_list);
            }
            catch(Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
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

                var bookDto = _mapper.Map<BookDto>(book);
                double ratingAverage = book.Ratings.Average(r => r.Score);
                bookDto.Rating = ratingAverage == 0 ? null : ratingAverage;

                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var book = _mapper.Map<Book>(bookDto);
                await _unitOfWork.Books.Add(book);
                await _unitOfWork.Save();

                var bookResultDto = _mapper.Map<BookDto>(book);
                return CreatedAtRoute("GetBook", new { id = book.Id }, bookResultDto);
            }
            catch(Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;
                //logging
                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}", Name ="UpdateBook")]
        public async Task<IActionResult> UpdateBook(int? id, [FromBody] BookDtoForUpdation bookDto)
        {
            if (id == null || id < 1) return BadRequest("Book id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
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
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }

        [Authorize]
        [HttpDelete("{id:int}", Name="DeleteBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteBook(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest("Book id should be defined, and have a valid value");
            }
            try
            {
                var book = await _unitOfWork.Books.Get(b => b.Id == id);

                if (book == null) return NotFound();

                _unitOfWork.Books.Delete(book.Id);
                await _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;
                //logging
                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }

        [Authorize]
        [HttpPost("rate/{id:int}", Name = "RateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RateBook(int? id,[FromBody] RateBookDto rateBookDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (id == null || id < 1)
            {
                return BadRequest("Book id should be defined, and have a valid value");
            }
            try
            {
                if (userId is null) {
                    throw new Exception("UserId is null");
                }

                var book = await _unitOfWork.Books.Get(b => b.Id == id, includes: new List<string> { "Ratings" });

                if (book == null) return NotFound();

                BookScore bookScoreOfUser = await _unitOfWork.Ratings
                    .Get(r => r.UserId == userId && r.BookId == book.Id);

                if (bookScoreOfUser != null)
                {
                    bookScoreOfUser.Score = rateBookDto.Score;
                    _unitOfWork.Ratings.Update(bookScoreOfUser);
                    await _unitOfWork.Save();
                    return Ok();
                }

                bookScoreOfUser = new BookScore();
                bookScoreOfUser.Score = rateBookDto.Score;
                bookScoreOfUser.BookId = book.Id;
                bookScoreOfUser.UserId = userId;

                await _unitOfWork.Ratings.Add(bookScoreOfUser);
                await _unitOfWork.Save();
                return Ok();
            }
            catch (Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }
    }
}
