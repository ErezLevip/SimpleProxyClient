using SimpleProxyClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SimpleProxyClient.Models
{
    internal class InvokeHttpRequest
    {
        public InvokeHttpRequest(string url, HttpRequestParameters parameters, Attributes.HttpMethod httpMethod)
        {
            Url = url;
            Parameters = parameters;
            HttpMethod = httpMethod;
        }
        public string Url { get; set; }
        public HttpRequestParameters Parameters { get; set; }
        public Attributes.HttpMethod HttpMethod { get; set; }
    }
}
