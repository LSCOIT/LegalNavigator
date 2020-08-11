using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Access2Justice.Api.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Access2Justice.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHostingEnvironment _env;

        public ErrorHandlingMiddleware(RequestDelegate next, IHostingEnvironment env)
        {
            _next = next;
            _env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);

            }
            catch (Exception ex)
            {

                if (_env.IsDevelopment())
                {
                    throw;
                }
                else
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await context.WriteResponse(new ErrorResponse(ex.Source, ex.Message));
                }
            }

        }
    }


    public static class ErrorHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseErrorHandlingMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ErrorHandlingMiddleware>();
            return app;
        }

        public static Task WriteResponse(this HttpContext context, ErrorResponse errorResponse)
        {
            errorResponse.TraceId = context.TraceIdentifier;
            var result = JsonConvert.SerializeObject(errorResponse);
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }

    }
    public class ErrorResponse
    {
        private readonly List<Error> _errors;

        public ErrorResponse()
        {
            _errors = new List<Error>();
        }

        public ErrorResponse(string source, string message, string code = null)
            : this(new Error(source, message, code))
        {
        }

        public ErrorResponse(Error error)
            : this(new[] { error })
        {
        }

        public ErrorResponse(IEnumerable<Error> errors)
        {
            _errors = errors.ToList();
        }

        public string TraceId { get; set; }

        public IReadOnlyCollection<Error> Errors => _errors.AsReadOnly();

        public void AddError(string source, string message, string code)
        {
            _errors.Add(new Error(source, message, code));
        }
    }

    public class Error
    {
        public Error(string source, string message, string code = null)
        {
            Source = source;
            Message = message;
            Code = code;
        }

        public string Code { get; }
        public string Message { get; }
        public string Source { get; }
    }
}
