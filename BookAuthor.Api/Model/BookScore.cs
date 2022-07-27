namespace BookAuthor.Api.Model
{
    public class BookScore
    {
        public int BookId { get; set; }
        public Book Book { get; set; }
        public string UserId { get; set; }
        public ApiUser User { get; set; }
        public int Score { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
