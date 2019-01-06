using System;

namespace Kin.Stellar.Sdk.requests
{
    public class ClientProtocolException : Exception
    {
        public ClientProtocolException(string message)
            : base(message) { }
    }
}