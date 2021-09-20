namespace ToDo.API.Const
{
    public static class ResponseMessage
    {
        // Common
        public const string ValidationError = "One or more validation errors occurred";
        public const string Unauthorized = "You do not have permission. Please, log in first";
        public const string NotFound = "Resource not found";
        public const string ResourceUpdatedSuccessfully = "Resource updated successfully";
        
        // Authentication
        public const string SignedUpSuccessfully = "Signed up successfully";
        public const string UserAlreadyExists = "User already exists";
        public const string LoggedOutSuccessfully = "Logged out successfully";
        public const string LoggedInSuccessfully = "Logged in successfully";
        public const string UserNotExist = "User not exist";
        public const string RefreshedTokensSuccessfully = "Tokens have been refreshed successfully";
        public const string RefreshTokenNotExist = "Refresh token does not exist";
        public const string RefreshTokenIsInvalid = "Refresh token is invalid or expired";
        
        // Profile
        public const string GotCurrentProfileSuccessfully = "Got current profile successfully";
        
        // To-Do
        public const string CreatedToDoSuccessfully = "Created To-Do successfully";
        public const string GotAllUserToDosSuccessfully = "Got all user to-do's successfully";
        public const string GotToDoByIdFromUserSuccessfully = "Got to-do successfully";
    }
}