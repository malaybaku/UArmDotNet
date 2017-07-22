using System;

namespace Baku.UArmDotNet
{
    public class UArmException : Exception
    {
        public UArmException() { }
        public UArmException(string message) : base(message) { }
        public UArmException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UArmErrorResponseException : UArmException
    {
        public UArmErrorResponseException() { }
        public UArmErrorResponseException(string message) : base(message) { }
        public UArmErrorResponseException(string message, Exception innerException) : base(message, innerException) { }
    }

    public class UArmNoResponseException : UArmException
    {
        public UArmNoResponseException() { }
    }


    public static class UArmExceptionFactory
    {
        public static UArmErrorResponseException CreateExceptionFromResponse(UArmResponse res)
        {
            return new UArmErrorResponseException($"ID:{res.Id}, Args=" + string.Join(", ", res.Args));
        }
    }


}
