using System.Net;

namespace Unipag
{
    public class ProcessingErrorException : Exception
    {
        public ProcessingErrorException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
