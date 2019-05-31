using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace Access2Justice.Api
{
    public static class ApplicationPipelineExtensions
    {
        /// <summary>
        /// Prevent removing cors headers on internal server error (500) in asp net core
        /// </summary>
        /// <remarks>https://github.com/aspnet/AspNetCore/issues/2378#issuecomment-354673591</remarks>
        public static IApplicationBuilder UseForcedCorsHeaders(this IApplicationBuilder builder)
        {
            return builder.Use(async (context, next) =>
            {
                // Find and hold onto any CORS related headers ...
                var corsHeaders = new HeaderDictionary();
                foreach (var (key, value) in context.Response.Headers)
                {
                    if (!key.ToLower().StartsWith("access-control-"))
                    {
                        continue; // Not CORS related
                    }
                    corsHeaders[key] = value;
                }

                // Bind to the OnStarting event so that we can make sure these CORS headers are still included going to the client
                context.Response.OnStarting(objectContext => {
                    var startingContext = (HttpContext)objectContext;
                    var headers = startingContext.Response.Headers;
                    // Ensure all CORS headers remain or else add them back in ...
                    foreach (var (key, value) in corsHeaders)
                    {
                        if (headers.ContainsKey(key))
                        {
                            continue;
                        }
                        headers.Add(key, value);
                    }
                    return Task.CompletedTask;
                }, context);

                // Call the pipeline ...
                await next();
            });
        }

        public static IApplicationBuilder UseFrameworkSecurity(this IApplicationBuilder builder)
        {
            return builder.Use(async (context, next) =>
            {
                context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
                context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
                await next();
            });
        }
    }
}
