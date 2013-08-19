using System.Net;

namespace Unipag
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
