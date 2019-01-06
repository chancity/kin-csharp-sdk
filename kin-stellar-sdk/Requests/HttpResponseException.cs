using System;

namespace Kin.Stellar.Sdk.requests
{
    public class HttpResponseException : Exception
    {
        public int StatusCode { get; set; }

        public HttpResponseException(int statusCode, string s)
            : base(s)
        {
            StatusCode = statusCode;
        }
    }
}