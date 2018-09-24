using SimpleProxyClient.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SimpleProxyClient.Abstractions
{
    public interface IServiceProxyClient<T>
    {
        T GetService();
        T GetService(ServiceInformation<T> serviceInformation);
        T GetService(Dictionary<string, string> headers, IOrderedEnumerable<IProxyInterceptor> interceptors = null);
        T GetService(Dictionary<string, string> headers = null, ServiceInformation<T> serviceInformation = null, IOrderedEnumerable<IProxyInterceptor> interceptors = null);
    }
}
