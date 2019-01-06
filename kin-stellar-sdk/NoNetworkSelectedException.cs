using System;
using System.Runtime.Serialization;

namespace Kin.Stellar.Sdk
{
    [Serializable]
    internal class NoNetworkSelectedException : Exception
    {
        public NoNetworkSelectedException() { }

        public NoNetworkSelectedException(string message) : base(message) { }

        public NoNetworkSelectedException(string message, Exception innerException) : base(message, innerException) { }

        protected NoNetworkSelectedException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}