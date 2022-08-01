namespace BookAuthor.Api.Exceptions
{
    public class ConflictEntityException : BaseException
    {
        public ConflictEntityException() : base()
        {
        }
        public ConflictEntityException(string message) : base(message)
        {
        }
        public ConflictEntityException(string message, List<ErrorContainer> errors) : base(message, errors)
        {
        }
    }
}

