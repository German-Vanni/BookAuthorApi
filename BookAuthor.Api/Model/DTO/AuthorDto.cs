using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Model.DTO
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string HomePlace { get; set; }
        public string Country { get; set; }
        public IEnumerable<BookDtoForNesting> Books { get; set; }
    }

    public class AuthorDtoForCreation
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Name is over the limit")]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string HomePlace { get; set; }
        public string Country { get; set; }
    }

    public class AuthorDtoForUpdation
    {
        [Required]
        [StringLength(maximumLength: 100, ErrorMessage = "Name is over the limit")]
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string HomePlace { get; set; }
        public string Country { get; set; }
    }

    public class AuthorDtoForNesting
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string HomePlace { get; set; }
        public string Country { get; set; }

    }
}
