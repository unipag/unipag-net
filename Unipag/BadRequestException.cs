using System.Net;

namespace Unipag
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
