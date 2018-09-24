using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SimpleProxyClient.Exceptions
{
    public class ProxyHttpException : HttpRequestException
    {
        const string ERROR_MESSAGE_TEMPLATE = "Response status code does not indicate success: {0} ({1}).";
        public ProxyHttpException(HttpStatusCode httpStatusCode, string message, Exception innter = null)
            : base(string.Format(ERROR_MESSAGE_TEMPLATE, httpStatusCode, message), innter)
        {
        }
    }
}
