namespace BookAuthor.Api.Exceptions
{
    public class BadRequestException : BaseException
    {
        public BadRequestException() : base()
        {
        }
        public BadRequestException(string message) : base(message)
        {
        }
        public BadRequestException(string message, List<ErrorContainer> errors) : base(message, errors)
        {
        }
    }
}

