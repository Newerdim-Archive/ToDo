using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ToDo.UnitTests.TestHelpers
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Get cookie from <see cref="HttpContext"/> response
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="key">Cookie key</param>
        /// <returns>Cookie if exists; otherwise, null</returns>
        public static string GetCookieFromResponse(this HttpContext httpContext, string key)
        {
            httpContext.Response.Headers.TryGetValue("Set-Cookie", out var cookies);

            return cookies.FirstOrDefault(x => x.StartsWith(key));
        }
    }
}