namespace Halcyon.Api.Services.Response
{
    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }
    }
}