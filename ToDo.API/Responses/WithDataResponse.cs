namespace ToDo.API.Responses
{
    public class WithDataResponse<T> : BaseResponse
    {
        public T Data { get; init; }
    }
}