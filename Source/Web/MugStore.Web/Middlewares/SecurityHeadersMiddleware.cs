using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace MugStore.Web.Middlewares
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // https://www.meziantou.net/security-headers-in-asp-net-core.htm


            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Referrer-Policy
            context.Response.Headers.Add("Referrer-Policy", new StringValues("strict-origin-when-cross-origin"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Frame-Options
            context.Response.Headers.Add("X-Frame-Options", new StringValues("DENY"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-Content-Type-Options
            context.Response.Headers.Add("X-Content-Type-Options", new StringValues("nosniff"));

            // https://security.stackexchange.com/questions/166024/does-the-x-permitted-cross-domain-policies-header-have-any-benefit-for-my-websit
            context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", new StringValues("none"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/X-XSS-Protection
            context.Response.Headers.Add("X-XSS-Protection", new StringValues("1; mode=block"));

            // https://developer.mozilla.org/en-US/docs/Web/HTTP/CSP
            // https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Content-Security-Policy
            //context.Response.Headers.Add("Content-Security-Policy", new StringValues(
            //    "script-src 'self' 'nonce-32e73ffe80' 'nonce-a4f06ab47b' 'nonce-f2036e568a' 'nonce-cf7c8ada5d' 'nonce-c1917c566a' 'nonce-03c2c1afb4' *.googletagmanager.com  *.google-analytics.com *.google.com *.googleapis.com https://connect.facebook.net https://www.facebook.com *.facebook.com; " +
            //    "connect-src 'self' graph.facebook.com; " +
            //    "img-src 'self' https://www.facebook.com"));

            await _next(context);
        }
    }
}
