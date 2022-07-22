namespace BookAuthor.Api.Model
{
    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Isbn10 { get; set; }
        public string Isbn13 { get; set; }

        public Author Author { get; set; }
        public int AuthorId { get; set; }
    }
}
