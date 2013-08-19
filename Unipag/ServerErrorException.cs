using System.Net;

namespace Unipag
{
    public class ServerErrorException : Exception
    {
        public ServerErrorException(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message, httpStatusCode, unipagError)
        {
        }
    }
}
