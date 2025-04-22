using Domain;
using BusinessLogicLayer.Services.Interfaces;
using System.Text;

namespace PresentationLayer.Middleware
{
    public class HttpLogMiddleware
    {
        private readonly RequestDelegate _next;

        public HttpLogMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var loggingService = context.RequestServices.GetRequiredService<IHttpLogService>();

            // --- Reading the Request Body ---
            context.Request.EnableBuffering();
            string requestBody = "";
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))    
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0; 
            }

            // --- Response thread substitution ---
            var originalResponseBody = context.Response.Body;
            using var responseBodyStream = new MemoryStream();
            context.Response.Body = responseBodyStream;

            // --- Calling the following middleware ---
            await _next(context);

            // --- Reading the Response Body ---
            responseBodyStream.Seek(0, SeekOrigin.Begin);
            string responseBody = await new StreamReader(responseBodyStream).ReadToEndAsync();
            responseBodyStream.Seek(0, SeekOrigin.Begin);

            // --- Logging into the database ---
            var log = new ApplicationLog
            {
                Timestamp = DateTime.UtcNow,
                Path = context.Request.Path,
                Method = context.Request.Method,
                QueryString = context.Request.QueryString.ToString(),
                RequestBody = requestBody,
                ResponseBody = responseBody,
                StatusCode = context.Response.StatusCode
            };
            await loggingService.LogAsync(log);

            // --- Returning a response to the client ---
            await responseBodyStream.CopyToAsync(originalResponseBody);
            context.Response.Body = originalResponseBody;
        }
    }
}
