using BookAuthor.Api.Exceptions;
using Newtonsoft.Json;
using System.Net;

namespace BookAuthor.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        const string _ERROR_500_MSG = "Internal Server Error. Please try again later!";

        public object HtppStatusCode { get; private set; }

        public ErrorHandlingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext context, IWebHostEnvironment environment)
        {
            var _logger = _loggerFactory.CreateLogger<ErrorHandlingMiddleware>();
            try
            {
                await _next(context);
            }
            catch(Exception ex)
            {
                await HandleExceptionAsync(context, ex, environment, _logger);
            }
        }

        private Task HandleExceptionAsync(
            HttpContext context, 
            Exception ex, 
            IWebHostEnvironment environment,
            ILogger<ErrorHandlingMiddleware> _logger
        )
        {
            _logger.LogError(ex.Message);
            string message;
            int statusCode;

            switch (ex)
            {
                case EntityNotFoundException e:
                    message = e.Message;
                    statusCode = (int) HttpStatusCode.NotFound;
                    break;
                case ConflictEntityException e:
                    message = e.Message;
                    statusCode = (int)HttpStatusCode.Conflict;
                    break;
                case UnauthorizedUserException e:
                    message = e.Message;
                    statusCode = (int)HttpStatusCode.Unauthorized;
                    break;
                default:
                    if (environment.IsDevelopment()) message = ex.Message;
                    else message = _ERROR_500_MSG;
                    statusCode = (int) HttpStatusCode.InternalServerError; ;
                    break;
            }


            var jsonResult = JsonConvert.SerializeObject(new
            {
                error = message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsync(jsonResult);
        }
    }
}
