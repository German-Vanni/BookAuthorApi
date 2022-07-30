using BookAuthor.Api.Util.Attributes;
using System.ComponentModel.DataAnnotations;

namespace BookAuthor.Api.Model.DTO
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [StringLength(10, MinimumLength = 10)]
        public string Isbn10 { get; set; }
        [StringLength(13, MinimumLength = 13)]
        public string Isbn13 { get; set; }
        public int PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
        public double? Rating { get; set; }
        public List<AuthorDtoForNesting> Authors { get; set; }
    }

    public class BookDtoForNesting
    {
        public int Id { get; set; }
        public string Title { get; set; }
        [StringLength(10, MinimumLength = 10)]
        public string Isbn10 { get; set; }
        [StringLength(13, MinimumLength = 13)]
        public string Isbn13 { get; set; }
        public int PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
        public double? Rating { get; set; }
    }

    public class BookDtoForCreation
    {
        [Required]
        [StringLength(maximumLength: 32768)]
        public string Title { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [ISBN(ISBNType.ISBN10)]
        public string Isbn10 { get; set; }

        [StringLength(13, MinimumLength = 13)]
        [ISBN(ISBNType.ISBN13)]
        public string Isbn13 { get; set; }

        [Range(1, 5000)]
        public int? PageCount { get; set; }

        [DateBeforeNow(allowNull: true)]
        public DateTime? PublicationDate { get; set; }

        [StringLength(maximumLength: 512)]
        public string Publisher { get; set; }

        [Required]
        [MinLength(1)]
        public List<int> AuthorIds { get; set; }
    }

    public class BookDtoForUpdation
    {
        [Required]
        [StringLength(maximumLength: 32768)]
        public string Title { get; set; }

        [StringLength(10, MinimumLength = 10)]
        [ISBN(ISBNType.ISBN10)]
        public string Isbn10 { get; set; }

        [StringLength(13, MinimumLength = 13)]
        [ISBN(ISBNType.ISBN13)]
        public string Isbn13 { get; set; }

        [Range(1, 5000, ErrorMessage = "Should be a valid positive integer")]
        public int PageCount { get; set; }

        [DateBeforeNow(allowNull: true)]
        public DateTime? PublicationDate { get; set; }

        [StringLength(maximumLength: 512)]
        public string Publisher { get; set; }

        [Required]
        [MinLength(1)]
        public List<int> AuthorIds { get; set; }
    }
}
