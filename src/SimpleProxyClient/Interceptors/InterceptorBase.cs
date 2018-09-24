using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Interceptors
{
    public abstract class HttpInterceptorBase<T>
    {
        public ISerializer Serializer { get; set; }
        public ServiceInformation<T> ServiceInformation { get; set; }
        public Headers Headers { get; set; }
        public IProxyInterceptor Next { get; set; }
    }
}
