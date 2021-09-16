namespace ToDo.API.Helpers
{
    public class TokenSettings
    {
        public string AccessTokenSecret { get; init; }
        public string RefreshTokenSecret { get; set; }
    }
}