using AutoMapper;
using BookAuthor.Api.ActionFilters;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;
using BookAuthor.Api.Services.BookService;
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
        private readonly IBookService _bookService;

        public BookController(
            IWebHostEnvironment environment,
            IUnitOfWork unitOfWork,
            IBookService bookService,
            IMapper mapper,
            ILogger<BookController> logger,
            UserManager<ApiUser> userManager)
            : base(environment, logger, userManager)
        {
            _bookService = bookService;
        }

        [HttpGet(Name = "GetBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks([FromQuery] RequestParameters requestParameters)
        {

            var bookDto_list = await _bookService.GetAllBooks_Paged(requestParameters.PageNumber, requestParameters.PageSize);
            return Ok(bookDto_list);

        }

        [HttpGet("{id:int}", Name = "GetBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBook(int id)
        {

            var bookDto = await _bookService.GetBook(id);

            return Ok(bookDto);

        }
        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
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

            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            bool approveBook = await IsUserInRoles(user, "Admin");

            var createdBook = await _bookService.CreateBook(bookDto, approveBook);

            return CreatedAtRoute("GetBook", new { id = createdBook.Id }, createdBook);

        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}", Name = "UpdateBook")]
        public async Task<IActionResult> UpdateBook(int? id, [FromBody] BookDtoForUpdation bookDto)
        {
            ApiUser user;

            if (id == null || id < 1) return BadRequest("Book id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            user = await GetClaimedUser();

            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            await _bookService.UpdateBook((int)id, bookDto);

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpDelete("{id:int}", Name = "DeleteBook")]
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

            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            await _bookService.DeleteBook((int)id);

            return Ok();
        }

        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost("rate/{id:int}", Name = "RateBook")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RateBook(int? id, [FromBody] RateBookDto rateBookDto)
        {
            ApiUser user;

            if (id == null || id < 1)
            {
                return BadRequest("Book id should be defined, and have a valid value");
            }

            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            await _bookService.RateBook(rateBookDto, (int)id, user);

            return Ok();

        }
        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpGet("approve", Name = "GetUnapprovedBooks")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnapprovedBooks([FromQuery] RequestParameters requestParameters)
        {
            ApiUser user;

            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            var books = await _bookService.GetUnapprovedBooks_Paged(requestParameters.PageNumber, requestParameters.PageSize);
            return Ok(books);
        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
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


            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            await _bookService.ApproveBook((int)id);

            return Ok();

        }
    }
}
