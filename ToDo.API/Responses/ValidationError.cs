using System.Collections.Generic;

namespace ToDo.API.Responses
{
    public class ValidationError
    {
        public string Property { get; init; }
        public IEnumerable<string> Messages { get; init; }
    }
}