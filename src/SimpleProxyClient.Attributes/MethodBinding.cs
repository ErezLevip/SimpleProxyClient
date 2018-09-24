using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class MethodBinding : Attribute
    {
        public InterceptorType InterceptorType { get; set; }
        public string Controller { get; set; }
        public string Name { get; set; }
        public HttpMethod HttpMethod { get; set; }

        public MethodBinding(string name = null,
            InterceptorType interceptor = InterceptorType.RestHttpInterceptor,
            HttpMethod httpMethod = HttpMethod.Post,
            string controller = null)
        {
            InterceptorType = InterceptorType;
            Name = name;
            HttpMethod = httpMethod;
            Controller = controller;
        }
    }
}
