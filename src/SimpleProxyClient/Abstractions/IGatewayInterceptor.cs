using Castle.DynamicProxy;
using SimpleProxyClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleProxyClient.Abstractions
{
    public interface IGatewayInterceptor<T> : IInterceptor
    {
        ISerializer Serializer { get; set; }
        ServiceInformation<T> ServiceInformation { get; set; }
        Headers Headers { get; set; }
        IOrderedEnumerable<IProxyInterceptor> GlobalInterceptors { get; set; }
    }
}
