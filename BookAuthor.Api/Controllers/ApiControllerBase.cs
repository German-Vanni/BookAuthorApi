using BookAuthor.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookAuthor.Api.Controllers
{
    [Route("api/[controller]")]
    public class ApiControllerBase<T> : Controller where T : ApiControllerBase<T> 
    {
        protected readonly IWebHostEnvironment _environment;
        protected readonly ILogger<T> _logger;
        protected readonly UserManager<ApiUser> _userManager;

        protected const string ERROR_500_MSG = "Internal Server Error.Please try again later";

        public ApiControllerBase(
            IWebHostEnvironment environment,
            ILogger<T> logger,
            UserManager<ApiUser> userManager)
        {
            _environment = environment;
            _logger = logger;
            _userManager = userManager;
        }

        protected async Task<ApiUser> GetClaimedUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApiUser user = await _userManager.FindByIdAsync(userId);
            return user;
        }

        protected IActionResult ClaimedUserNotFound()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            _logger.LogInformation("User with UserId: {0} was not found", userId);
            return Unauthorized("Invalid sign in credentials");
        }

        protected async Task<bool> IsUserInRoles(ApiUser user, string roles)
        {
            var userRoles = await _userManager.GetRolesAsync(user);
            foreach(string roleToTest in roles.Split(' '))
            {
                if (!userRoles.Contains(roleToTest)) return false;
            }
            return true;
        }
        protected IActionResult LogServerError(Exception ex)
        {
            string message = ERROR_500_MSG;
            if (_environment.IsDevelopment()) message = ex.Message;

            _logger.LogError(ex, ex.Message);
            return StatusCode(500, message);
        }
    }
}
