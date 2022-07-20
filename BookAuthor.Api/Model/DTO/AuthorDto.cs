using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Model.DTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Name is over the limit" )]
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public IEnumerable<BookDto> Books { get; set; }
    }

    public class AuthorDtoForCreation
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Name is over the limit")]
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
    }
}
