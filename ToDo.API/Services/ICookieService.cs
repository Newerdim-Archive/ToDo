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

        /// <summary>
        ///     Delete cookie
        /// </summary>
        /// <param name="key">Cookie key</param>
        public void Delete(string key);

        /// <summary>
        ///     Get value from cookie
        /// </summary>
        /// <param name="key">Cookie key</param>
        /// <returns>Cookie value if exists; otherwise, null</returns>
        public string GetValue(string key);
    }
}