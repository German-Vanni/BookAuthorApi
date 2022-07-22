using BookAuthor.Api.Model.DTO;

namespace BookAuthor.Api.Services.AuthManager
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(UserDtoForLogin userDto);
        Task<string> CreateToken();
    }
}
