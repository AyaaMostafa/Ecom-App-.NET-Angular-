using Ecom.API.Helper;
using Microsoft.Extensions.Caching.Memory;
using System.Net;
using System.Text.Json;

namespace Ecom.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostEnvironment _environment;
        private readonly IMemoryCache _memoryCache;
        private readonly TimeSpan _rateLimitWindow = TimeSpan.FromSeconds(30);

        public ExceptionMiddleware(RequestDelegate next, IHostEnvironment environment, IMemoryCache memoryCache) //processing on req if not ok
        {
            _next = next;
            _environment = environment;
            _memoryCache = memoryCache;
        }
        public async Task InvokeAsync(HttpContext context)  
        {
            try
            {
                ApplySecurity(context);
                if (IsRequestAllowed(context)==false)
                {

                    context.Response.StatusCode = (int)HttpStatusCode.TooManyRequests;
                    context.Response.ContentType = "application/json";
                    var response = new ApiException((int)HttpStatusCode.TooManyRequests, "Too many requests. Please try again later.");
                    await context.Response.WriteAsJsonAsync(response);
                      return;

                }
                await _next(context); //call next middleware



            }
            catch (Exception ex)
            {

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var response = _environment.IsDevelopment() ?
                    new ApiException((int)HttpStatusCode.InternalServerError, ex.Message, ex.StackTrace.ToString())
                    : new ApiException((int)HttpStatusCode.InternalServerError, ex.Message);
                var json = JsonSerializer.Serialize(response); //translate to json
                await context.Response.WriteAsync(json);
            }

        }
        private bool IsRequestAllowed(HttpContext context)
        {

            var ip = context.Connection.RemoteIpAddress.ToString();
            var cashKey = $"Rate: {ip}";    
            var dateNow = DateTime.Now;

            var (timesTamp, count)= _memoryCache.GetOrCreate(cashKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = _rateLimitWindow;
                return (timesTamp: dateNow, count: 0);
            });
            if(dateNow - timesTamp < _rateLimitWindow) //timestamp:last request received or reched to server
            {
                if(count >= 8) //max 5 req in 30 sec
                {
                    return false;
                }
                _memoryCache.Set(cashKey, (timesTamp, count + 1), _rateLimitWindow);
            }
            else
            {
                _memoryCache.Set(cashKey, (timesTamp, count), _rateLimitWindow);
            }
            return true;
        }   
        private void ApplySecurity(HttpContext context)
        {
            context.Response.Headers["X-Content-Type-Options"] = "nosniff";
            context.Response.Headers["X-XSS-Protection"] = "1; mode=block";
            context.Response.Headers["X-Frame-Options"] = "DENY";
        }
    }
}
