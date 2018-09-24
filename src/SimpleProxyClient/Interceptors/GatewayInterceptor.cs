using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SimpleProxyClient.Attributes;
using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Models;
using System.Net.Http;
using SimpleProxyClient.Extentions;
using Castle.DynamicProxy;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SimpleProxyClient.Interceptors
{
    public class GatewayInterceptor<T> : HttpInterceptorBase<T>, IGatewayInterceptor<T>
    {
        private static Dictionary<InterceptorType, Type> _interceptors;
        public IOrderedEnumerable<IProxyInterceptor> GlobalInterceptors { get; set; }

        static GatewayInterceptor()
        {
            _interceptors = typeof(GatewayInterceptor<T>).Assembly.GetTypes().Select(t =>
            {
                return new
                {
                    type = t,
                    attribute = t.GetCustomAttribute<Attributes.Interceptor>()
                };
            }).Where(t => t.attribute != null).ToDictionary(t => t.attribute.InterceptorType, t => t.type);
        }

        public GatewayInterceptor(ISerializer serializer, ServiceInformation<T> serviceInfo)
        {
            Serializer = serializer;
            ServiceInformation = serviceInfo;
        }

        public void Intercept(IInvocation invocation)
        {
            invocation.Proceed();
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var targetMethodInfo = invocation.ExtractTargetMethodInfo<T>();
            var interceptorType = _interceptors[targetMethodInfo.InterceptorType];
            var genericType = interceptorType.MakeGenericType(typeof(T));

            var factory = (IHttpInterceptorFactory<T>)Activator.CreateInstance(genericType);
            var callInterceptor = factory.Create(Serializer, ServiceInformation, targetMethodInfo, Headers);
            sw.Stop();
            Console.WriteLine($"interception preperation took {sw.ElapsedTicks}");

            if (invocation.Method.ReturnType == typeof(Task))
            {
                invocation.ReturnValue = CreateSuspendedTaskForExecution(invocation, callInterceptor);
            }
            else if (invocation.Method.ReturnType.IsGenericType && invocation.Method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>))
            {
                invocation.ReturnValue = CreateSuspendedTaskForExecution((dynamic)invocation.ReturnValue, invocation, callInterceptor);
            }
            else
            {
                var firstInterceptor = ChainInterceptors(GlobalInterceptors, callInterceptor);
                firstInterceptor.Proceed(invocation);
            }
        }

        private Task<ReturnType> CreateSuspendedTaskForExecution<ReturnType>(Task<ReturnType> originalTask, IInvocation invocation, IProxyInterceptor callInterceptor)
        {
            return Task.Run(async () =>
            {
                var firstInterceptor = ChainInterceptors(GlobalInterceptors, callInterceptor);
                firstInterceptor.Proceed(invocation);
                return await (Task<ReturnType>)invocation.ReturnValue;
            });
        }

        private Task CreateSuspendedTaskForExecution(IInvocation invocation, IProxyInterceptor callInterceptor)
        {
            return Task.Run(() =>
            {
                var firstInterceptor = ChainInterceptors(GlobalInterceptors, callInterceptor);
                firstInterceptor.Proceed(invocation);
            });
        }

        private IProxyInterceptor ChainInterceptors(IOrderedEnumerable<IProxyInterceptor> globalInterceptors, IProxyInterceptor callInterceptor)
        {
            var globalInterceptorsArray = globalInterceptors.ToArray();
            for (int i = 0; i < globalInterceptorsArray.Length; i++)
            {
                GetNextInterceptor(i, globalInterceptorsArray, callInterceptor).Next = GetNextInterceptor(i + 1, globalInterceptorsArray, callInterceptor);
            }

            return globalInterceptorsArray.First();
        }

        private IProxyInterceptor GetNextInterceptor(int i, IProxyInterceptor[] globalInterceptors, IProxyInterceptor callInterceptor)
        {
            if (i == globalInterceptors.Length)
                return callInterceptor;
            if (i > globalInterceptors.Length)
                return null;
            return globalInterceptors[i];
        }
        private static bool IsTask(Type invocationMethodReturnType)
        {
            return invocationMethodReturnType == typeof(Task) || invocationMethodReturnType.IsGenericType && invocationMethodReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }
    }
}
