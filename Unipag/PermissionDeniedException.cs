using System.Net;

namespace Unipag
{
    public class PermissionDeniedException : Exception
    {
        public PermissionDeniedException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
