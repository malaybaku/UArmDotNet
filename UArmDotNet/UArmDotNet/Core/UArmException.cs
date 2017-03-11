using System;

namespace Baku.UArmDotNet
{
    public class UArmException : Exception
    {
        public UArmException() { }
        public UArmException(string message) : base(message) { }
        public UArmException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UArmCommandException : UArmException
    {
        public UArmCommandException() { }
        public UArmCommandException(string message) : base(message) { }
        public UArmCommandException(string message, Exception innerException) : base(message, innerException) { }
    }

    public static class UArmExceptionFactory
    {
        public static UArmCommandException CreateExceptionFromResponse(UArmResponse res)
        {
            throw new NotImplementedException();
        }
    }


}
