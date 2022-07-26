using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Model.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public int? PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
        public IEnumerable<AuthorDtoForNesting> Authors { get; set; }
    }

    public class BookDtoForNesting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public int? PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
    }

    public class BookDtoForCreation
    {
        [Required]
        [StringLength(maximumLength: 32768)]
        public string Title { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public int? PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
        public List<int> AuthorIds { get; set; }
    }

    public class BookDtoForUpdation
    {
        [Required]
        [StringLength(maximumLength: 32768)]
        public string Title { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public int? PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
        public List<int> AuthorIds { get; set; }
    }
}
