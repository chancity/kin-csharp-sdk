using System;

namespace Kin.Stellar.Sdk.federation
{
    public class ConnectionErrorException : Exception
    {
        public ConnectionErrorException() { }

        public ConnectionErrorException(string message)
            : base(message) { }
    }
}