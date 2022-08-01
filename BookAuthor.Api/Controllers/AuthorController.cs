using AutoMapper;
using BookAuthor.Api.ActionFilters;
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

        public AuthorController(
            IWebHostEnvironment environment,
            IAuthorService authorService,
            ILogger<AuthorController> logger,
            UserManager<ApiUser> userManager)
            : base(environment, logger, userManager)
        {
            _authorService = authorService;
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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
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

            bool approved = await IsUserInRoles(user, "Admin");

            var authorResultDto = await _authorService.CreateAuthor(authorDto, approved);

            return CreatedAtRoute("GetAuthor", new { id = authorResultDto.Id }, authorResultDto);

        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPut("{id:int}", Name = "UpdateAuthor")]
        public async Task<IActionResult> UpdateAuthor(int? id, [FromBody] AuthorDtoForUpdation authorDto)
        {
            if (id == null || id < 1) return BadRequest("Author id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _authorService.UpdateAuthor((int)id, authorDto);

            return NoContent();

        }

        [Authorize(Roles = "Admin")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpDelete("{id:int}", Name = "DeleteAuthor")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteAuthor(int? id)
        {
            if (id == null || id < 1)
            {
                return BadRequest("Author id should be defined, and have a valid value");
            }

            await _authorService.DeleteAuthor((int)id);

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
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [HttpPost("approve/{id:int}", Name = "ApproveAuthor")]
        public async Task<IActionResult> ApproveAuthor(int? id)
        {

            if (id == null || id < 1) return BadRequest("Author id should be defined, and have a valid value");
            if (!ModelState.IsValid) return BadRequest(ModelState);

            await _authorService.ApproveAuthor((int)id);

            return Ok();

        }
    }
}
