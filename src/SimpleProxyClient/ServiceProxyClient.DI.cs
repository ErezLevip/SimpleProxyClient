using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;
using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient
{
    public static class ServiceProxyClientDI
    {
        public static IServiceCollection AddSimpleProxyClient<T>(this IServiceCollection serviceCollection) where T : class
        {
            return serviceCollection.AddScoped<IGatewayInterceptor<T>, GatewayInterceptor<T>>()
            .AddScoped<IServiceProxyClient<T>, ServiceProxyClient<T>>();
        }
    }
}
