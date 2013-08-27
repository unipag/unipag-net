using System.Net;

namespace Unipag
{
    public class ServiceUnavailableException : Exception
    {
        public ServiceUnavailableException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
