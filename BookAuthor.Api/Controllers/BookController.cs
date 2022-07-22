using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookAuthor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly ILogger<BookController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookController(IUnitOfWork unitOfWork,
            IMapper mapper, 
            ILogger<BookController> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetBooks()
        {
            try
            {
                _logger.Log(LogLevel.Error, "TESTING MESSAGE");
                var books = await _unitOfWork.Books.GetAll();
                var bookDto_list = _mapper.Map<IEnumerable<Book>, IEnumerable<BookDto>>(books);
                return Ok(bookDto_list);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
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
                var book = await _unitOfWork.Books.Get(b => b.Id == id, new List<string> { "Author"});
                if(book == null)
                {
                    return NotFound();
                }
                var bookDto = _mapper.Map<BookDto>(book);
                return Ok(bookDto);
            }
            catch (Exception ex)
            {
                //logging
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost]
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
                //logging
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error. Please try again later");
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
                //logging
                _logger.LogError(ex.Message);
                return StatusCode(500, "Internal Server Error. Please try again later");
            }
        }

        [Authorize]
        [HttpDelete("{id:int}", Name="DeleteBook")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
                //logging
                _logger.LogError(ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}
