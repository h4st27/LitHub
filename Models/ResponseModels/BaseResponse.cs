﻿using System.Net;

namespace MyApp.Models.ResponseModels
{
    public class BaseResponse<T>
    {
        public string? Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
    };
}
