namespace BookAuthor.Api.Model
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDay { get; set; }
        public virtual IEnumerable<Book> Books { get; set; }
    }
}
