using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace WebAPI.Exception
{
    public class GlobleExceptionHndler : IExceptionHandler
    {
        private readonly ILogger<GlobleExceptionHndler> _logger;

        public GlobleExceptionHndler(ILogger<GlobleExceptionHndler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, 
            System.Exception exception, CancellationToken cancellationToken)
        {
            if (exception is TimeoutException)
            {
                await MyException.ExceptionMessage(httpContext, exception,
              HttpStatusCode.RequestTimeout, "A timeout occurred");

                return true;
            }

            if (exception is ArgumentException)
            {

                await MyException.ExceptionMessage(httpContext, exception,
                             HttpStatusCode.BadRequest, "A bad request occurred");
                return true;
            }
            else
            {
                await MyException.ExceptionMessage(httpContext, exception,
                    HttpStatusCode.InternalServerError, "An " +
                    "unexpected error occurred");
                return true;
            }

            return false;
        }
    }
}
