using System;
using System.Net;

namespace Mockingjay
{
    [Serializable]
    public class MockingjayClientException : Exception
    {
        public MockingjayClientException(HttpStatusCode statusCode)
        {
            StatusCode = statusCode;
        }
        public MockingjayClientException(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
        public MockingjayClientException(HttpStatusCode statusCode, string message, Exception inner) : base(message, inner)
        {
            StatusCode = statusCode;
        }
        protected MockingjayClientException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }

        public HttpStatusCode StatusCode { get; }
    }
}
