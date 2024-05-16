using System.Net;

namespace Libra.Models.ResponseModels
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public T? Data { get; set; }
    };
}
