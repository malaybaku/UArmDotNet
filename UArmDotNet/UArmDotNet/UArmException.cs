using System;

namespace Baku.UArmDotNet
{
    public class UArmException : Exception
    {
        public UArmException() { }
        public UArmException(string message) : base(message) { }
        public UArmException(string message, Exception innerException) : base(message, innerException) { }
    }
}
