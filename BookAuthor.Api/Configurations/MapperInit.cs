using AutoMapper;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;

namespace BookAuthor.Api.Configurations
{
    public class MapperInit : Profile
    {
        public MapperInit()
        {
            CreateMap<Author, AuthorDto>().ReverseMap();
            CreateMap<Author, AuthorDtoForCreation>().ReverseMap();
            CreateMap<Author, AuthorDtoForUpdation>().ReverseMap();
            CreateMap<UserDtoForCreation, ApiUser>();
            CreateMap<UserDto, ApiUser>().ReverseMap(); 
            CreateMap<Book, BookDto>().ReverseMap();
            CreateMap<Book, BookDtoForCreation>().ReverseMap();
            CreateMap<Book, BookDtoForUpdation>().ReverseMap();



        }
    }
}
