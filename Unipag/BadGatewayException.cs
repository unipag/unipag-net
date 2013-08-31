using System.Net;

namespace Unipag
{
    public class BadGatewayException : Exception
    {
        public BadGatewayException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
