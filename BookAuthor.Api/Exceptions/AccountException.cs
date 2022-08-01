namespace BookAuthor.Api.Exceptions
{
    public class AccountException : BaseException
    {
        public AccountException() : base()
        {
        }
        public AccountException(string message) : base(message)
        {
        }
        public AccountException(string message, List<ErrorContainer> errors) : base (message, errors)
        {
        }
    }
}
