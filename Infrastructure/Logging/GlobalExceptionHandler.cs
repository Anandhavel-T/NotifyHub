using System.Net.Http;
using System.Net;
using System.Web.Http.ExceptionHandling;

namespace NotifyHub.Infrastructure.Logging
{
    public class GlobalExceptionHandler : ExceptionHandler
    {
        private readonly ILoggerService _logger;

        public GlobalExceptionHandler(ILoggerService logger)
        {
            _logger = logger;
        }

        public override void Handle(ExceptionHandlerContext context)
        {
            _logger.LogError("An unhandled exception occurred.", context.Exception);

            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("An unexpected error occurred."),
                ReasonPhrase = "Internal Server Error"
            };

            context.Result = new ErrorMessageResult(context.Request, response);
        }
    }
}