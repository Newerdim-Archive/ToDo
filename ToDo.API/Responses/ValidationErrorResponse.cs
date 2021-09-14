using System.Collections.Generic;

namespace ToDo.API.Responses
{
    public class ValidationErrorResponse : BaseResponse
    {
        public IEnumerable<ValidationError> Errors { get; init; }
    }
}