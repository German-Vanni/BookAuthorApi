using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;
using BookAuthor.Api.Model.Paging;

namespace BookAuthor.Api.Services.AuthorService
{
    public interface IAuthorService
    {
        Task<AuthorDto> GetAuthor(int id);
        Task<List<AuthorDto>> GetAllAuthors_Paged(int pageNumber, int pageSize);
        Task<AuthorDto> CreateAuthor(AuthorDtoForCreation authorDto, bool Approved = false);
        Task UpdateAuthor(int authorId, AuthorDtoForUpdation authorDto);
        Task DeleteAuthor(int id);
        Task<List<AuthorDto>> GetUnapprovedAuthors_Paged(int pageNumber, int pageSize);
        Task ApproveAuthor(int id);
    }
}
