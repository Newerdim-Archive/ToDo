namespace ToDo.API.Services
{
    public interface ICookieService
    {
        /// <summary>
        ///     Add cookie to response
        /// </summary>
        /// <param name="key">Cookie key</param>
        /// <param name="value">Cookie value</param>
        public void Add(string key, string value);
    }
}