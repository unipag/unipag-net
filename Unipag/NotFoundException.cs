using System.Net;

namespace Unipag
{
    public class NotFoundException : Exception
    {
        public NotFoundException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
