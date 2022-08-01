using AutoMapper;
using BookAuthor.Api.DataAccess.Repository.UnitOfWork;
using BookAuthor.Api.Exceptions;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;

namespace BookAuthor.Api.Services.AuthorService
{
    public class AuthorService : IAuthorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AuthorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AuthorDto> GetAuthor(int id)
        {
            var author = await _unitOfWork.Authors.Get(a => a.Id == id, new List<string> { "AuthorBooks.Book" });
            if (author == null)
            {
                throw new EntityNotFoundException();
            }
            if (!author.Approved)
            {
                throw new ConflictEntityException("Author is pending approval");
            }

            var authorDto = _mapper.Map<AuthorDto>(author);
            return authorDto;
        }

        public async Task<AuthorDto> CreateAuthor(AuthorDtoForCreation authorDto, bool Approved = false)
        {
            var author = _mapper.Map<Author>(authorDto);

            //automatically approve if user is an admin
            author.Approved = Approved;

            await _unitOfWork.Authors.Add(author);
            await _unitOfWork.Save();

            var authorResultDto = _mapper.Map<AuthorDto>(author);
            return authorResultDto;
        }

        public async Task UpdateAuthor(int authorId, AuthorDtoForUpdation authorDto)
        {
            var author = await _unitOfWork.Authors.Get(a => a.Id == authorId);
            if (author == null)
            {
                throw new EntityNotFoundException("Author does not exist");
            }

            _mapper.Map(authorDto, author);
            _unitOfWork.Authors.Update(author);
            await _unitOfWork.Save();

            return;
        }

        public async Task DeleteAuthor(int id)
        {
            var author = await _unitOfWork.Authors.Get(a => a.Id == id);

            if (author == null)
            {
                throw new EntityNotFoundException("Author does not exist");
            }

            _unitOfWork.Books.Delete(author.Id);
            await _unitOfWork.Save();

            return;
        }

        public async Task<List<AuthorDto>> GetAllAuthors_Paged(int pageNumber, int pageSize)
        {
            var authors = await _unitOfWork.Authors
                .GetPaged(
                pageNumber, pageSize,
                a => a.Approved);
            var authorsDtos = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(authors).ToList();

            return authorsDtos;
        }

        public async Task<List<AuthorDto>> GetUnapprovedAuthors_Paged(int pageNumber, int pageSize)
        {
            var authors = await _unitOfWork.Authors
                .GetPaged(
                pageNumber, pageSize,
                a => a.Approved == false
                );

            var authorsDto_list = _mapper.Map<IEnumerable<Author>, IEnumerable<AuthorDto>>(authors).ToList();

            return authorsDto_list;
        }

        public async Task ApproveAuthor(int id)
        {
            var author = await _unitOfWork.Authors.Get(a => a.Id == (int)id);
            if (author == null)
            {
                throw new EntityNotFoundException("Author does not exist");
            }

            author.Approved = true;

            _unitOfWork.Authors.Update(author);
            await _unitOfWork.Save();
        }
    }
}
