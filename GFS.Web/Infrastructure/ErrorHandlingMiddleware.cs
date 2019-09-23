using System.Net;
using System.Threading.Tasks;
using GFS.Core;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace GFS.Web.Infrastructure
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
           _next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await _next(context);
            }
            catch (GfsException ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, GfsException exception)
        {
            if (exception != null)
            {
                var code = HttpStatusCode.BadRequest;
                var result = JsonConvert.SerializeObject(new {error = exception.Message});
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int) code;
                return context.Response.WriteAsync(result);
            }

            return context.Response.WriteAsync("Null Exception");
        }
    }
}