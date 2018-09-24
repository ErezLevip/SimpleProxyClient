using SimpleProxyClient.Attributes;
using SimpleProxyClient.Interceptors;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleProxyClient.Models
{
    internal class TargetMethodInfo
    {
        public string Url { get; set; }
        public object[] Args { get; set; }
        public HttpMethod HttpMethod { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public InterceptorType InterceptorType { get; set; }
    }
}
