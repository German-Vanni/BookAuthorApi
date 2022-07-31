using Newtonsoft.Json;
using System.Net;

namespace BookAuthor.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        const string _ERROR_500_MSG = "Internal Server Error. Please try again later!";

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
            string message = _ERROR_500_MSG;
            if (environment.IsDevelopment()) message = ex.Message;

            var code = HttpStatusCode.InternalServerError;
            var jsonResult = JsonConvert.SerializeObject(new
            {
                error = message
            });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(jsonResult);
        }
    }
}
