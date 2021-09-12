using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace ToDo.UnitTests.TestHelpers
{
    public static class HttpContextExtensions
    {
        /// <summary>
        /// Get cookie from response
        /// </summary>
        /// <param name="httpContext">Http context</param>
        /// <param name="key">Cookie key</param>
        /// <returns>Cookie if exists; otherwise, null</returns>
        public static string GetCookieFromResponse(this HttpContext httpContext, string key)
        {
            httpContext.Response.Headers.TryGetValue("Set-Cookie", out var cookies);

            return cookies.FirstOrDefault(x => x.StartsWith(key));
        }
        
        /// <summary>
        /// Get cookie value from response
        /// </summary>
        /// <param name="httpContext">Http context</param>
        /// <param name="cookieKey">Cookie key</param>
        /// <returns>Cookie value if cookie exists; otherwise, null</returns>
        public static string GetCookieValueFromResponse(this HttpContext httpContext, string cookieKey)
        {
            var cookie = httpContext.GetCookieFromResponse(cookieKey);
            
            // Cookie looks like this: key=value; path=/; secure; samesite=none; httponly
            // This will get only the value from cookie
            return cookie?.Split(new[] {"=", ";"}, StringSplitOptions.None)[1];
        }
    }
}