using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Attributes;
using SimpleProxyClient.Models;
using System;
using System.Reflection;
using System.Linq;
using SimpleProxyClient.Interceptors;
using System.Collections.Generic;
using Castle.DynamicProxy;
using Moq;
using LinFu.DynamicProxy;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace SimpleProxyClient
{
    public class ServiceProxyClient<T> : IServiceProxyClient<T> where T : class
    {
        private readonly IGatewayInterceptor<T> _gatewayInterceptor;
        private static ConcurrentDictionary<Type, object> _clients = new ConcurrentDictionary<Type, object>();
        private static ConcurrentDictionary<Type, object> _clientFakeTargets = new ConcurrentDictionary<Type, object>();
        private static object _lockObj = new object();

        public ServiceProxyClient(IGatewayInterceptor<T> gatewayInterceptor)
        {
            ValidateT();

            _gatewayInterceptor = gatewayInterceptor;
        }

        private void ValidateT()
        {
            if (!typeof(T).IsInterface)
                throw new InvalidOperationException("T must be an interface");
        }

        public T GetService()
        {
            return ServiceProxyFactory();
        }

        public T GetService(IOrderedEnumerable<IProxyInterceptor> interceptors)
        {
            return ServiceProxyFactory(interceptors);
        }

        public T GetService(Dictionary<string, string> headers, IOrderedEnumerable<IProxyInterceptor> interceptors = null)
        {
            return ServiceProxyFactory(interceptors, headers);
        }

        public T GetService(ServiceInformation<T> serviceInformation)
        {
            return ServiceProxyFactory(null, null, serviceInformation);
        }

        public T GetService(Dictionary<string, string> headers = null, ServiceInformation<T> serviceInformation = null, IOrderedEnumerable<IProxyInterceptor> interceptors = null)
        {
            return ServiceProxyFactory(interceptors, headers, serviceInformation);
        }

        private T ServiceProxyFactory(IOrderedEnumerable<IProxyInterceptor> interceptors = null, 
            Dictionary<string, string> headers = null, ServiceInformation<T> serviceInformation = null)
        {
            Stopwatch sp = new Stopwatch();
            sp.Start();
            var clientType = typeof(T);
            if (_clients.ContainsKey(clientType))
                return _clients[clientType] as T;

            T proxy = null;
            lock (_lockObj)
            {
                if (_clients.ContainsKey(clientType))
                    return _clients[clientType] as T;

                proxy = CreateInterceptor(interceptors, new Headers(headers), serviceInformation, clientType);
                _clients.TryAdd(clientType, proxy);
            }

            sp.Stop();
            //Console.WriteLine($"proxy creation time {sp.ElapsedMilliseconds}");
            return proxy;
        }

        private T CreateInterceptor(IOrderedEnumerable<IProxyInterceptor> interceptors, Headers headers, ServiceInformation<T> serviceInformation, Type clientType)
        {
            var serviceInfo = serviceInformation != null ? serviceInformation : _gatewayInterceptor.ServiceInformation;
            _gatewayInterceptor.Headers = headers;
            _gatewayInterceptor.GlobalInterceptors = interceptors;

            var mock = new Mock<T>();
            var pg = new ProxyGenerator();
            return pg.CreateInterfaceProxyWithTarget<T>(GetTarget(clientType), _gatewayInterceptor);
        }

        private T GetTarget(Type t)
        {
            if (_clientFakeTargets.TryGetValue(t, out object target))
                return target as T;

            var newMock = new Mock<T>().Object;
            _clientFakeTargets.TryAdd(t, newMock);

            return newMock;
        }
    }
}
