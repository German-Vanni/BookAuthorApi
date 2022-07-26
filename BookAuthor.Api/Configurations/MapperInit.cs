using AutoMapper;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;

namespace BookAuthor.Api.Configurations
{
    public class MapperInit : Profile
    {
        public MapperInit()
        {
            CreateMap<Author, AuthorDto>()
                .ForMember(dto => dto.Books, 
                    opt => opt.MapFrom(a => a.AuthorBooks.Select(ab => ab.Book).ToList()));
            CreateMap<Author, AuthorDtoForNesting>();
            CreateMap<Author, AuthorDtoForCreation>().ReverseMap();
            CreateMap<Author, AuthorDtoForUpdation>().ReverseMap();

            CreateMap<UserDtoForCreation, ApiUser>();
            CreateMap<UserDto, ApiUser>().ReverseMap();

            CreateMap<Book, BookDto>()
                .ForMember(dto => dto.Authors,
                    opt => opt.MapFrom(b => b.AuthorBooks.Select(ab => ab.Author).ToList()));
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookDtoForNesting>();
            CreateMap<Book, BookDtoForCreation>().ReverseMap();
            CreateMap<Book, BookDtoForUpdation>().ReverseMap();



        }
    }
}
