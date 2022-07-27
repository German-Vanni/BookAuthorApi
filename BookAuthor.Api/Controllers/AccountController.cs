using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Services.AuthManager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BookAuthor.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<BookController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        private readonly string USER_ROLE_NAME;
        public AccountController(
            IWebHostEnvironment environment,
            IConfiguration configuration,
            ILogger<BookController> logger,
            IUnitOfWork unitOfWork,
            IMapper mapper,
             IAuthManager authManager, 
            UserManager<ApiUser> userManager
            )
        {
            _environment = environment;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _authManager = authManager;
            _userManager = userManager;
            _logger = logger;

            USER_ROLE_NAME = configuration.GetSection("Roles").GetSection("User").GetSection("Name").Value;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] UserDtoForCreation userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var user = _mapper.Map<ApiUser>(userDto);
                user.UserName = userDto.Email;

                var identityResult = await _userManager.CreateAsync(user, userDto.Password);
                if (!identityResult.Succeeded)
                {
                    foreach(var e in identityResult.Errors.ToList())
                    {
                        ModelState.AddModelError(e.Code, e.Description);
                    }
                    return BadRequest(ModelState);
                }

                await _userManager.AddToRoleAsync(user, USER_ROLE_NAME);
                return Accepted();
            }
            catch (Exception ex)
            {
                string message = "Internal Server Error.Please try again later";
                if (_environment.IsDevelopment()) message = ex.Message;

                _logger.LogError(ex.Message);
                return StatusCode(500, message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] UserDtoForLogin userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                if (!await _authManager.ValidateUser(userDto))
                {
                    return Unauthorized("Invalid sign in credentials");
                }

                return Accepted(new { Token = await _authManager.CreateToken()});
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
