using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace Unipag
{
    public class UnipagExceptionFactory
    {
        public Exception Exception(Error unipagError, HttpStatusCode httpStatusCode)
        {
            string exceptionMessage = unipagError.Description;

            if (unipagError.Params != null && unipagError.Params.Count > 0)
            {
                exceptionMessage = string.Format("{0} Params: [{1}]", exceptionMessage, JsonConvert.SerializeObject(unipagError.Params));
            }

            if (httpStatusCode == HttpStatusCode.InternalServerError)
                return new InternalServerErrorException(exceptionMessage, httpStatusCode, unipagError);

            switch ((int)httpStatusCode)
            {
                case 400:
                    return new BadRequestException(exceptionMessage, httpStatusCode, unipagError);
                case 401:
                    return new UnauthorizedException(exceptionMessage, httpStatusCode, unipagError);
                case 403:
                    return new ForbiddenException(exceptionMessage, httpStatusCode, unipagError);
                case 404:
                    return new NotFoundException(exceptionMessage, httpStatusCode, unipagError);
                case 405:
                    return new MethodNotAllowedException(exceptionMessage, httpStatusCode, unipagError);
                case 500:
                    return new InternalServerErrorException(exceptionMessage, httpStatusCode, unipagError);
                case 502:
                    return new BadGatewayException(exceptionMessage, httpStatusCode, unipagError);
                case 503:
                    return new ServiceUnavailableException(exceptionMessage, httpStatusCode, unipagError);
                default:
                    return new Exception(exceptionMessage, httpStatusCode, unipagError);
            }
        }
    }

    public class Exception : ApplicationException
    {
        public HttpStatusCode HttpStatusCode { get; set; }
        public Error UnipagError { get; set; }
        public Dictionary<string, string> Params { get; set; }

        public Exception()
        {
        }

        public override System.Collections.IDictionary Data
        {
            get
            {
                return Params;
            }
        }

        public Exception(string message, HttpStatusCode httpStatusCode, Error unipagError)
            : base(message)
        {
            HttpStatusCode = httpStatusCode;
            UnipagError = unipagError;
            Params = unipagError.Params;
        }
    }
}
