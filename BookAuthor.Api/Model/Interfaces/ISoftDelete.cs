namespace BookAuthor.Api.Model.Interfaces
{
    public interface ISoftDelete
    {
        public bool Deleted { get; set; }
        public DateTimeOffset? DeletedAt { get; set; }
    }
}
