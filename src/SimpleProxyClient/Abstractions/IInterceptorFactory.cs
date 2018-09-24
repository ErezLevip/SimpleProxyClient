using Castle.DynamicProxy;
using SimpleProxyClient.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Abstractions
{
    internal interface IHttpInterceptorFactory<T>
    {
        ISerializer Serializer { get; set; }
        ServiceInformation<T> ServiceInformation { get; set; }
        Headers Headers { get; set; }
        IProxyInterceptor Create(ISerializer serializer, ServiceInformation<T> serviceInfo,TargetMethodInfo targetMethodInfo, Headers headers);
    }
}
