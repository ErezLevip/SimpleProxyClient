using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Abstractions
{
    public interface IProxyInterceptor
    {
        void Proceed(IInvocation invocation);
        IProxyInterceptor Next { get; set; }
    }
}
