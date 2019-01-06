using System;

namespace Kin.Stellar.Sdk.requests
{
    public class TooManyRequestsException : Exception
    {
        public int RetryAfter { get; set; }

        public TooManyRequestsException(int retryAfter)
            : base("The rate limit for the requesting IP address is over its alloted limit.")
        {
            RetryAfter = retryAfter;
        }
    }
}