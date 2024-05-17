using System.Net;

namespace LitHub.Models.ResponseModels
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    };
}
