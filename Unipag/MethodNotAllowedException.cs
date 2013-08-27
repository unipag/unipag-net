using System.Net;

namespace Unipag
{
    public class MethodNotAllowedException : Exception
    {
        public MethodNotAllowedException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
