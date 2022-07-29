using BookAuthor.Api.Model.Interfaces;

namespace BookAuthor.Api.Model
{
    public class Author : ISoftDelete, IApproved
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? BirthDate { get; set; }
        public DateTime? DeathDate { get; set; }
        public string ImageUrl { get; set; }
        public string About { get; set; }
        public string HomePlace { get; set; }
        public string Country { get; set; }
        public ICollection<AuthorBook> AuthorBooks { get; set; }

        public bool Deleted { get; set; } = false;
        public DateTimeOffset? DeletedAt { get; set; } = null;
        public bool Approved { get; set; } = false;
    }
}
