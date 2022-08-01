using BookAuthor.Api.Exceptions;
using BookAuthor.Api.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace BookAuthor.Api.ActionFilters
{
    public class ValidationFilterAttribute : IAsyncActionFilter
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly ILogger<ValidationFilterAttribute> _logger;

        public ValidationFilterAttribute(UserManager<ApiUser> userManager, ILogger<ValidationFilterAttribute> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate _next)
        {
            //find out if claimed user exists
            var userId = context.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            ApiUser user = await _userManager.FindByIdAsync(userId);
            if (user is null)
            {
                _logger.LogInformation("User with UserId: {0} was not found", userId);
                throw new UnauthorizedUserException("Invalid sign in credentials");
            }

            var result = await _next();
        }

    }
}
