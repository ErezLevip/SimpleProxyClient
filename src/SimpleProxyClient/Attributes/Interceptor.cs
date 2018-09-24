using SimpleProxyClient.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes
{
    internal class Interceptor : Attribute
    {
        public InterceptorType InterceptorType { get; set; }

        public Interceptor(InterceptorType interceptorType)
        {
            InterceptorType = interceptorType;
        }
    }
}
