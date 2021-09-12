using System;
using System.Linq;
using System.Net.Http;

namespace ToDo.IntegrationTests.Helpers
{
    public static class HttpResponseMessageExtensions
    {
        /// <summary>
        /// Get cookie from http response
        /// </summary>
        /// <param name="httpResponseMessage">Http response message</param>
        /// <param name="cookieKey">Cookie key</param>
        /// <returns>Cookie if exists; otherwise, null</returns>
        public static string GetCookie(this HttpResponseMessage httpResponseMessage, string cookieKey)
        {
            httpResponseMessage.Headers.TryGetValues("Set-Cookie", out var cookies);

            return cookies?.FirstOrDefault(x => x.StartsWith(cookieKey));
        }
        
        /// <summary>
        /// Get cookie value from http response
        /// </summary>
        /// <param name="httpResponseMessage">Http response message</param>
        /// <param name="cookieKey">Cookie key</param>
        /// <returns>Cookie value if cookie exists; otherwise, null</returns>
        public static string GetCookieValue(this HttpResponseMessage httpResponseMessage, string cookieKey)
        {
            var cookie = httpResponseMessage.GetCookie(cookieKey);
            
            // Cookie looks like this: key=value; path=/; secure; samesite=none; httponly
            // This will get only the value from cookie
            return cookie?.Split(new[] {"=", ";"}, StringSplitOptions.None)[1];
        }
    }
}