using AutoMapper;
using BookAuthor.Api.Exceptions;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Services.AuthManager;
using Microsoft.AspNetCore.Identity;

namespace BookAuthor.Api.Services.AccountService
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountService> _logger;
        private readonly IAuthManager _authManager;
        private readonly string USER_ROLE_NAME;

        public AccountService(
            IMapper mapper, 
            UserManager<ApiUser> userManager, 
            IConfiguration configuration,
            ILogger<AccountService> logger,
            IAuthManager authManager
            )
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _authManager = authManager;
            USER_ROLE_NAME = configuration.GetSection("Roles").GetSection("User").GetSection("Name").Value;
        }

        public async Task Register(UserDtoForCreation userDto)
        {

            var user = _mapper.Map<ApiUser>(userDto);
            user.UserName = userDto.Email;

            var identityResult = await _userManager.CreateAsync(user, userDto.Password);
            if (!identityResult.Succeeded)
            {
                List<ErrorContainer> Errors = new();
                foreach (var e in identityResult.Errors.ToList())
                {
                    Errors.Add(new ErrorContainer()
                    {
                        Code = e.Code,
                        Description = e.Description
                    });
                }
                throw new AccountException("Error occured on registration", Errors);
            }

            await _userManager.AddToRoleAsync(user, USER_ROLE_NAME);
            return;
        }

        public async Task<string> Login(UserDtoForLogin userDto)
        {
            ApiUser user = await _userManager.FindByEmailAsync(userDto.Email);
            if (user is null)
            {
                _logger.LogInformation("User with Email: {0} was not found", userDto.Email);
                throw new  UnauthorizedUserException("Invalid sign in credentials");
            }

            if (!await _authManager.ValidateUser(userDto))
            {
                throw new UnauthorizedUserException("Invalid sign in credentials");
            }

            return await _authManager.CreateToken();
        }
    }
}
