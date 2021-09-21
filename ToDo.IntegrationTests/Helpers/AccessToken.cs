namespace ToDo.IntegrationTests.Helpers
{
    public static class AccessToken
    {
        /// <summary>
        ///     Valid access token with user id = 1
        /// </summary>
        public const string Valid =
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiIxIiwibmJmIjoxNjMxNzE4MzY5LCJleHAiOjE3MzE3MTkyNjksImlhdCI6MTYzMTcxODM2OX0.3RwcGu3lxeZ0rmb_oP_H0BQDTteG0rRKFA9Q2w04UXoTSsBy4srIwd24PXcte6CnYWAWPs8TkwkebNYZEzHCtw";

        /// <summary>
        ///     Invalid access token
        /// </summary>
        public const string Invalid = "invalid";

        /// <summary>
        ///     Valid access token but with user id = 999
        /// </summary>
        public const string WithNotExistingUser =
            "eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiI5OTkiLCJuYmYiOjE2MzE3MTgzNjksImV4cCI6MTczMTcxOTI2OSwiaWF0IjoxNjMxNzE4MzY5fQ.ljq75wGhsJkG-tAtKl1ezp6ZYG7gSjruwVie_WNtBV7UsS6MyBtAvBY5NWH9nfAJaf6ID8jWUlpyXm4Ix09ZLw";
    }
}