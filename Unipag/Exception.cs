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
                return new ServerErrorException(exceptionMessage, httpStatusCode, unipagError);

            switch (unipagError.Code)
            {
                case "unauthorized":
                    return new UnauthorizedException(exceptionMessage, httpStatusCode, unipagError);
                case "bad_params":
                    return new BadParamsException(exceptionMessage, httpStatusCode, unipagError);
                case "not_found":
                    return new NotFoundException(exceptionMessage, httpStatusCode, unipagError);
                case "permission_denied":
                    return new PermissionDeniedException(exceptionMessage, httpStatusCode, unipagError);
                case "processing_error":
                    return new ProcessingErrorException(exceptionMessage, httpStatusCode, unipagError);
                case "forbidden":
                    return new ForbiddenException(exceptionMessage, httpStatusCode, unipagError);
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
