namespace ToDo.API.Const
{
    public static class ResponseMessage
    {
        // Common
        public const string ActionPerformedSuccessfully = "Action performed successfully";
        public const string ResourceGottenSuccessfully = "Resource(s) gotten successfully";
        public const string ResourceUpdatedSuccessfully = "Resource(s) updated sucessfully";
        public const string ResourceCreatedSuccessfully = "Resource(s) created sucessfully";
        public const string ResourceDeletedSuccessfully = "Resource(s) deleted sucessfully";
        public const string ResourceNotFound = "Resource(s) not found";
        
        // Errors
        public const string UserNotExist = "User not exist";
        public const string RefreshTokenNotExist = "Refresh token does not exist";
        public const string RefreshTokenIsInvalid = "Refresh token is invalid or expired";
        public const string UserAlreadyExists = "User already exists";

        public const string ValidationError = "One or more validation errors occurred";
        public const string Unauthorized = "You do not have permission. Please, log in first";
        public const string UnexpectedError = "Unexpected error occurred, try again later";
    }
}