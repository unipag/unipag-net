using System.Net;

namespace Unipag
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
