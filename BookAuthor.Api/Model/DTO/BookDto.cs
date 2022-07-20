using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Model.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public int AuthorId { get; set; }
        public Author Author { get; set; }
    }

    public class BookDtoForCreation
    {
        [Required]
        [StringLength(maximumLength: 32768)]
        public string Title { get; set; }
        public string ISBN10 { get; set; }
        public string ISBN13 { get; set; }
        public int AuthorId { get; set; }
    }
}
