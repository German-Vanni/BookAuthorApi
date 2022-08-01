using BookAuthor.Api.Model.DTO;

namespace BookAuthor.Api.Services.AccountService
{
    public interface IAccountService
    {
        public Task Register(UserDtoForCreation userDto);
        public Task<string> Login(UserDtoForLogin userDto);
    }
}
