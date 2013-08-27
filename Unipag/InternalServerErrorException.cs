using System.Net;

namespace Unipag
{
    public class InternalServerErrorException : Exception
    {
        public InternalServerErrorException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
