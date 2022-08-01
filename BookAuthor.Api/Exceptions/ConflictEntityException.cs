namespace BookAuthor.Api.Exceptions
{
    public class ConflictEntityException : Exception
    {
        public ConflictEntityException() : base()
        {
        }
        public ConflictEntityException(string message) : base(message)
        {
        }
    }
}
