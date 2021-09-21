using Microsoft.AspNetCore.Http;

namespace ToDo.API.Services.Implementations
{
    public class CookieService : ICookieService
    {
        private readonly IRequestCookieCollection _requestCookies;
        private readonly IResponseCookies _responseCookies;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _responseCookies = httpContextAccessor.HttpContext?.Response.Cookies;
            _requestCookies = httpContextAccessor.HttpContext?.Request.Cookies;
        }

        public void Add(string key, string value)
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };

            _responseCookies.Append(key, value, cookieOptions);
        }

        public void Delete(string key)
        {
            var cookieOptions = new CookieOptions
            {
                Secure = true,
                HttpOnly = true,
                SameSite = SameSiteMode.None
            };

            _responseCookies.Delete(key, cookieOptions);
        }

        public string GetValue(string key)
        {
            _requestCookies.TryGetValue(key, out var value);

            return value;
        }
    }
}