namespace BookAuthor.Api.Model
{
    public class Book
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
    }
}
