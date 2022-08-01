using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;
using BookAuthor.Api.Services.AuthorService;
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
        private readonly IAuthorService _authorService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorController(
            IWebHostEnvironment environment,
            IAuthorService authorService,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<AuthorController> logger,
            UserManager<ApiUser> userManager)
            : base(environment, logger, userManager)
        {
            _authorService = authorService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet(Name = "GetAuthors")]
        public async Task<IActionResult> GetAuthors([FromQuery] RequestParameters requestParameters)
        {

            var authorsDtos = await _authorService.GetAllAuthors_Paged(requestParameters.PageNumber, requestParameters.PageSize);
            return Ok(authorsDtos);

        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpGet("{id:int}", Name = "GetAuthor")]
        public async Task<IActionResult> GetAuthor(int id)
        {
            var authorDto = await _authorService.GetAuthor(id);
            return Ok(authorDto);

        }

        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost(Name = "CreateAuthor")]
        public async Task<IActionResult> CreateAuthor([FromBody] AuthorDtoForCreation authorDto)
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

            bool approved = await IsUserInRoles(user, "Admin");

            var authorResultDto = _authorService.CreateAuthor(authorDto, approved);

            return CreatedAtRoute("GetAuthor", new { id = authorResultDto.Id }, authorResultDto);

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


            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            await _authorService.UpdateAuthor((int)id, authorDto);

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}", Name = "DeleteAuthor")]
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

            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            return NoContent();

        }

        [HttpGet("approve", Name = "GetUnapprovedAuthors")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUnapprovedAuthors([FromQuery] RequestParameters requestParameters)
        {

            var authorDto_list = await _authorService.GetUnapprovedAuthors_Paged(requestParameters.PageNumber, requestParameters.PageSize);

            return Ok(authorDto_list);

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


            user = await GetClaimedUser();
            if (user is null)
            {
                return ClaimedUserNotFound();
            }

            return Ok();

        }
    }
}
