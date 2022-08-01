namespace BookAuthor.Api.Exceptions
{
    public class EntityNotFoundException : BaseException
    {
        public EntityNotFoundException() : base()
        {
        }
        public EntityNotFoundException(string message) : base(message)
        {
        }
        public EntityNotFoundException(string message, List<ErrorContainer> errors) : base(message, errors)
        {
        }
    }
}

