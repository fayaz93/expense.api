using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Serko.Common.Log;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Serko.Common.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILogger logger)
        {
            this.next = next;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            var watch = new Stopwatch();
            watch.Start();

            var request = await FormatRequest(context.Request);
            logger.LogInformation<RequestResponseLoggingMiddleware>($"Request  : {request}");

            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;

                await next(context);

                var response = await FormatResponse(context.Response);
                logger.LogInformation<RequestResponseLoggingMiddleware>($"Response : {response}");

                await responseBody.CopyToAsync(originalBodyStream);
            }

            watch.Stop();
            logger.LogInformation<RequestResponseLoggingMiddleware>($"Processing time: {watch.ElapsedMilliseconds} ms");
        }

        private async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableRewind();

            var body = request.Body;
            var buffer = new byte[Convert.ToInt32(request.ContentLength)];

            await request.Body.ReadAsync(buffer, 0, buffer.Length);

            var text = Encoding.UTF8.GetString(buffer);
            body.Seek(0, SeekOrigin.Begin);
            request.Body = body;

            return $"{request.Scheme} {request.Host}{request.Path} {request.QueryString} {text}";
        }

        private async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }
    }
}
