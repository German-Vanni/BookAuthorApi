using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookAuthor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet]
        public async Task<IActionResult> GetAuthors()
        {
            try
            {
                var authors = await _unitOfWork.Authors.GetAll();
                var authorsDtos = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(authors);
                return Ok(authorsDtos);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            try
            {
                var author = await _unitOfWork.Authors.Get(a => a.Id == id, new List<string> { "Books"});
                var authorDto = _mapper.Map<AuthorDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
