using System;

namespace Kin.Jwt.Exceptions
{
    public class ConvertPemException : Exception
    {
        public ConvertPemException(string message) : base(message) { }
    }
}