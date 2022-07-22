using AutoMapper;
using BookAuthor.Api.Model;
using BookAuthor.Api.Model.DTO;

namespace BookAuthor.Api.Configurations
{
    public class MapperInit : Profile
    {
        public MapperInit()
        {
            CreateMap<Book, BookDto>().ReverseMap().MaxDepth(1);
            CreateMap<Book, BookDtoForCreation>().ReverseMap();
            CreateMap<Author, AuthorDto>().ReverseMap().MaxDepth(1);
            CreateMap<Author, AuthorDtoForCreation>().ReverseMap();
            CreateMap<UserDtoForCreation, ApiUser>();
            CreateMap<UserDto, ApiUser>().ReverseMap(); ;

        }
    }
}
