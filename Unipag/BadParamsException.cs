using System.Net;

namespace Unipag
{
    public class BadParamsException : Exception
    {
        public BadParamsException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
