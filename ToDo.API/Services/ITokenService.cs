namespace ToDo.API.Services
{
    public interface ITokenService
    {
        /// <summary>
        /// Create new access token
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Access token</returns>
        public string CreateAccessToken(int userId);

        /// <summary>
        /// Create new refresh token
        /// </summary>
        /// <param name="userId">User id</param>
        /// <returns>Refresh token</returns>
        public string CreateRefreshToken(int userId);
    }
}