namespace ToDo.API.Const
{
    public static class ResponseMessage
    {
        // Common
        public const string ActionPreformedSuccessfully = "Action preformed successfully";
        public const string ResourceGottenSuccessfully = "Resource(s) obtained sucessfully";
        public const string ResourceUpdatedSuccessfully = "Resource(s) updated sucessfully";
        public const string ResourceCreatedSuccessfully = "Resource(s) created sucessfully";
            
        // Errors
        public const string UserNotExist = "User not exist";
        public const string RefreshTokenNotExist = "Refresh token does not exist";
        public const string RefreshTokenIsInvalid = "Refresh token is invalid or expired";
        public const string UserAlreadyExists = "User already exists";
        public const string ValidationError = "One or more validation errors occurred";
        public const string Unauthorized = "You do not have permission. Please, log in first";
        public const string NotFound = "Resource not found";
        public const string UnexpectedError = "Unexpected error occurred, try again later";
    }
}