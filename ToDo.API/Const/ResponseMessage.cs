namespace ToDo.API.Const
{
    public static class ResponseMessage
    {
        public const string SignedUpSuccessfully = "Signed up successfully";
        public const string ValidationError = "One or more validation errors occurred";
        public const string UserAlreadyExists = "User already exists";
        public const string LoggedOutSuccessfully = "Logged out successfully";
        public const string Unauthorized = "You do not have permission. Please, log in first";
        public const string LoggedInSuccessfully = "Logged in successfully";
        public const string UserNotExist = "User not exist";
        public const string RefreshedTokensSuccessfully = "Tokens have been refreshed successfully";
        public const string RefreshTokenNotExist = "Refresh token does not exist";
        public const string RefreshTokenIsInvalid = "Refresh token is invalid or expired";
        public const string GotCurrentProfileSuccessfully = "Got current profile successfully";
        public const string CreatedToDoSuccessfully = "Created To-Do successfully";
    }
}