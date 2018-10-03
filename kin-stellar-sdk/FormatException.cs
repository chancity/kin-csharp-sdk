using System;
using System.Runtime.Serialization;

namespace Kin.Stellar.Sdk
{
    public class FormatException : Exception
    {
        public FormatException()
        {
        }

        public FormatException(string message) : base(message)
        {
        }

        public FormatException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected FormatException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}