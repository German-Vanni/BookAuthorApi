using BookAuthor.Api.Model.Interfaces;

namespace BookAuthor.Api.Model
{
    public class Book : ISoftDelete, IApproved
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }
        public int? PageCount { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string Publisher { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }
        public ICollection<BookScore> Ratings { get; set; }
        public bool Deleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; } = null;
        public bool Approved { get; set; } = false;
    }
}
