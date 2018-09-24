using SimpleProxyCore.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyCore.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodBinding : Attribute
    {
        public InterceptorType InterceptorType { get; set; }
        public string Controller { get; set; }
        public string Name { get; set; }
        public HttpMethods HttpMethod { get; set; }
    }
}
