using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace WebApplication5.Services
{
    public class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public ApiException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}