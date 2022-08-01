namespace BookAuthor.Api.Exceptions
{
    public class UnauthorizedUserException : BaseException
    {
        public UnauthorizedUserException() : base()
        {
        }
        public UnauthorizedUserException(string message) : base(message)
        {
        }
        public UnauthorizedUserException(string message, List<ErrorContainer> errors) : base(message, errors)
        {
        }
    }
}
