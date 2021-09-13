using Microsoft.AspNetCore.Http;

namespace ToDo.API.Services
{
    public class CookieService : ICookieService
    {
        private readonly IResponseCookies _responseCookies;

        public CookieService(IHttpContextAccessor httpContextAccessor)
        {
            _responseCookies = httpContextAccessor.HttpContext?.Response.Cookies;
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
    }
}