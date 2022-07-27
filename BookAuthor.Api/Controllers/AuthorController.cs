using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookAuthor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IHostEnvironment _environment;
        private readonly ILogger<AuthorController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorController(
            IHostEnvironment environment,
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<AuthorController> logger)
        {
            _environment = environment;
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet(Name = "GetAuthors")]
        public async Task<IActionResult> GetAuthors([FromQuery] RequestParameters requestParameters)
        {
            try
            {
                var authors = await _unitOfWork.Authors.GetAll(requestParameters);
                var authorsDtos = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(authors);
                return Ok(authorsDtos);
            }
            catch (Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:int}", Name ="GetAuthor")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                var author = await _unitOfWork.Authors.Get(a => a.Id == id, new List<string> { "AuthorBooks.Book" });
                if (author == null)
                {
                    return NotFound();
                }
                var authorDto = _mapper.Map<AuthorDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost(Name = "CreateAuthor")]
        public async Task<IActionResult> CreateAuthor([FromBody]AuthorDtoForCreation authorDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var author = _mapper.Map<Author>(authorDto);
                await _unitOfWork.Authors.Add(author);
                await _unitOfWork.Save();

                var authorResultDto = _mapper.Map<AuthorDto>(author);
                return CreatedAtRoute("GetAuthor", new { id = author.Id }, authorResultDto);
            }
            catch(Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}", Name = "UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(int? id, [FromBody] AuthorDtoForUpdation authorDto)
        {
            if (id == null || id < 1) return BadRequest("Author id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                var author = await _unitOfWork.Authors.Get(a => a.Id == (int)id);
                if (author == null)
                {
                    return NotFound();
                }

                _mapper.Map(authorDto, author);
                _unitOfWork.Authors.Update(author);
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
        [HttpDelete("{id:int}", Name="DeleteAuthor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest("Author id should be defined, and have a valid value");
            }
            try
            {
                var author = await _unitOfWork.Authors.Get(a => a.Id == id);

                if (author == null) return NotFound();

                _unitOfWork.Books.Delete(author.Id);
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
    }
}
