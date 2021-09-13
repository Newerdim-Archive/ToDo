using System;

namespace ToDo.API.Const
{
    public static class ResponseMessage
    {
        public const string SignedUpSuccessfully = "Signed up successfully";
        public const string TokenIsInvalid = "Token is invalid";
        public const string TokenNotHaveProfileInformation = "Token does not have profile informations";
        public const string UserAlreadyExists = "User already exists";
    }
}