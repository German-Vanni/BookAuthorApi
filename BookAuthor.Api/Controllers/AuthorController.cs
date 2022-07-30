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
    public class AuthorController : ApiControllerBase<AuthorController>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorController(
            IWebHostEnvironment environment,
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            ILogger<AuthorController> logger,
            UserManager<ApiUser> userManager) 
            :base(environment, logger, userManager)
        {
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
                var authors = await _unitOfWork.Authors
                    .GetPaged(
                    requestParameters, 
                    a => a.Approved);
                var authorsDtos = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(authors);
                return Ok(authorsDtos);
            }
            catch (Exception ex)
            {
                return LogServerError(ex);
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
                if (!author.Approved)
                {
                    return Conflict("Author is pending approval");
                }

                var authorDto = _mapper.Map<AuthorDto>(author);
                return Ok(authorDto);
            }
            catch (Exception ex)
            {
                return LogServerError(ex);
            }
        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost(Name = "CreateAuthor")]
        public async Task<IActionResult> CreateAuthor([FromBody]AuthorDtoForCreation authorDto)
        {
            ApiUser user;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var author = _mapper.Map<Author>(authorDto);

                //automatically approve if user is an admin
                author.Approved = await IsUserInRoles(user, "Admin");

                await _unitOfWork.Authors.Add(author);
                await _unitOfWork.Save();

                var authorResultDto = _mapper.Map<AuthorDto>(author);
                return CreatedAtRoute("GetAuthor", new { id = author.Id }, authorResultDto);
            }
            catch(Exception ex)
            {
                return LogServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}", Name = "UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(int? id, [FromBody] AuthorDtoForUpdation authorDto)
        {
            ApiUser user;

            if (id == null || id < 1) return BadRequest("Author id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

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
                return LogServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name="DeleteAuthor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor(int? id)
        {
            ApiUser user;

            if (id == null || id < 1)
            {
                return BadRequest("Author id should be defined, and have a valid value");
            }
            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var author = await _unitOfWork.Authors.Get(a => a.Id == id);

                if (author == null) return NotFound();

                _unitOfWork.Books.Delete(author.Id);
                await _unitOfWork.Save();
                return NoContent();
            }
            catch (Exception ex)
            {
                return LogServerError(ex);
            }
        }

        [HttpGet("approve", Name = "GetUnapprovedAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnapprovedAuthors([FromQuery] RequestParameters requestParameters)
        {
            try
            {
                var authors = await _unitOfWork.Authors
                    .GetPaged(
                    requestParameters,
                    a => a.Approved == false
                    );
                var authorDto_list = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(authors).ToList();

                return Ok(authorDto_list);
            }
            catch (Exception ex)
            {
                return LogServerError(ex);
            }
        }

        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("approve/{id:int}", Name = "ApproveAuthor")]
        public async Task<IActionResult> ApproveAuthor(int? id)
        {
            ApiUser user;

            if (id == null || id < 1) return BadRequest("Author id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                user = await GetClaimedUser();
                if (user is null)
                {
                    return ClaimedUserNotFound();
                }

                var author = await _unitOfWork.Authors.Get(a => a.Id == (int)id);
                if (author == null)
                {
                    return NotFound();
                }

                author.Approved = true;

                _unitOfWork.Authors.Update(author);
                await _unitOfWork.Save();

                return Ok();
            }
            catch (Exception ex)
            {
                return LogServerError(ex);
            }
        }
    }
}
