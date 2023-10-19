using Newtonsoft.Json;
using System.Net;

namespace MultiTenant.Api.Middlewares
{
    public class PathMiddleware
    {
        private readonly RequestDelegate _next;

        public PathMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                if (context.Request.Path.HasValue)
                {
                    var @params = context.Request.Path.Value.Split('/');
                    string slugTenant = @params.FirstOrDefault(x => x.Contains("slug_"));

                    if (!string.IsNullOrEmpty(slugTenant))
                    {
                        context.Items.Add(nameof(slugTenant), slugTenant);
                    }
                }
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var obj = new
            {
                Error = new
                {
                    message = exception.Message,
                    details = exception.InnerException?.Message
                }
            };

            string json = JsonConvert.SerializeObject(obj);
            return context.Response.WriteAsync(json);
        }
    }
}
