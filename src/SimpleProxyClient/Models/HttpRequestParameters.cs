using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Models
{
    internal class HttpRequestParameters
    {
        public Dictionary<string,string> QueryParameters { get; set; }
        public Dictionary<string,object> BodyParameters { get; set; }
        public Dictionary<string,string> HeadersParameters { get; set; }

        public HttpRequestParameters()
        {
            QueryParameters = new Dictionary<string, string>();
            BodyParameters = new Dictionary<string, object>();
            HeadersParameters = new Dictionary<string, string>();
        }
    }
}
