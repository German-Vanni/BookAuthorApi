namespace BookAuthor.Api.Model
{
    public class Author
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DeathDate { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string HomePlace { get; set; }
        public string Country { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }

    }
}
