namespace BookAuthor.Api.Exceptions
{
    public class BaseException : Exception
    {
        public List<ErrorContainer> Errors;
        public BaseException() : base()
        {
        }
        public BaseException(string message) : base()
        {
        }
        public BaseException(string message, List<ErrorContainer> errors) : base(message)
        {
            if (errors is not null && errors.Count > 0) Errors = errors;
        }
    }
}
