using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Events;
using System;
using System.Threading.Tasks;

namespace Api.Middlewares
{
    public class ErrorLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public ErrorLoggingMiddleware(RequestDelegate next)
        {
            if (next == null) throw new ArgumentNullException(nameof(next));

            this._next = next;
            this._logger = Log.ForContext<ErrorLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (httpContext == null) throw new ArgumentNullException(nameof(httpContext));

            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                this._logger.Write(LogEventLevel.Error, ex.Message, ex);
                //System.Diagnostics.Debug.WriteLine($"The following error happened: {ex.Message}");
                throw;
            }
        }
    }
}
