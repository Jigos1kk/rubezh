using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace RubezhGateway.Engine.Middleware
{
    public class DefaultMiddleware
    {
        private readonly RequestDelegate _next;
        public DefaultMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stowatch = Stopwatch.StartNew();
            context.Request.Headers["X-Rubezh-Gateway"] = "v1.0";
            await _next(context);
        }
    }

    public static class DefaultMiddlewareExtensions
    {
        public static IApplicationBuilder UseDefaultMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DefaultMiddleware>();
        }
    }
}