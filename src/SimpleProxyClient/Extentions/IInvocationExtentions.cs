using Castle.DynamicProxy;
using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Attributes;
using SimpleProxyClient.Models;
using SimpleProxyClient.Utils;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProxyClient.Extentions
{
    internal static class IInvocationExtentions
    {
        static ConcurrentDictionary<Type, ReflectedData> _cachedMethodReflectedData = new ConcurrentDictionary<Type, ReflectedData>();
        static readonly object _lockObj = new object();
        internal static TargetMethodInfo ExtractTargetMethodInfo<T>(this IInvocation invocation)
        {
            var type = typeof(T);

            if (!_cachedMethodReflectedData.TryGetValue(type, out ReflectedData reflectedData))
            {
                lock (_lockObj)
                {
                    reflectedData = GetReflectedDataFromCache(invocation, type);
                }
            }
            string controller = RetrieveControllerName(reflectedData);

            var name = !string.IsNullOrEmpty(reflectedData.MethodBinding.Name) ? reflectedData.MethodBinding.Name : invocation.Method.Name;
            return new TargetMethodInfo
            {
                Args = invocation.Arguments,
                Url = $"{controller}/{name}",
                HttpMethod = reflectedData.MethodBinding.HttpMethod,
                MethodInfo = reflectedData.MethodInfo,
                InterceptorType = reflectedData.InterceptorType
            };
        }

        private static string RetrieveControllerName(ReflectedData reflectedData)
        {
            var controller = reflectedData.MethodBinding.Controller;

            if (string.IsNullOrEmpty(controller))
            {
                if (reflectedData.ControllerBinding == null || string.IsNullOrEmpty(reflectedData.ControllerBinding.Controller))
                    throw new InvalidOperationException("Contract must have a controller");
                else
                    controller = reflectedData.ControllerBinding.Controller;
            }

            return controller;
        }

        private static ReflectedData GetReflectedDataFromCache(IInvocation invocation, Type type)
        {
            ReflectedData reflectedData;
            if (!_cachedMethodReflectedData.TryGetValue(type, out reflectedData))
            {
                reflectedData = new ReflectedData
                {
                    ControllerBinding = type.GetCustomAttribute<ControllerBinding>(),
                    MethodInfo = type.GetMethod(invocation.Method.Name)
                };

                reflectedData.MethodBinding = reflectedData.MethodInfo.GetCustomAttribute<MethodBinding>();
                _cachedMethodReflectedData.TryAdd(type, reflectedData);
            }

            return reflectedData;
        }

        internal static void InvokeHttpMethod(this IInvocation invocation, ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            var returnType = invocation.Method.ReturnType.IsGenericType ? invocation.Method.ReturnType.GetGenericTypeDefinition() : invocation.Method.ReturnType;
            switch (req.HttpMethod)
            {
                case Attributes.HttpMethod.Get:
                    if (invocation.MethodInvocationTarget.ReturnType == typeof(void))
                        RestInterceptorUtils.Get(serializer, headers, req);
                    else
                        invocation.ReturnValue = RestInterceptorUtils.Get(invocation.MethodInvocationTarget.ReturnType, serializer, headers, req);
                    return;
                case Attributes.HttpMethod.Post:
                    if (invocation.MethodInvocationTarget.ReturnType == typeof(void))
                        RestInterceptorUtils.Post(serializer, headers, req);
                    else
                        invocation.ReturnValue = RestInterceptorUtils.Post(invocation.MethodInvocationTarget.ReturnType, serializer, headers, req);
                    return;
                default:
                    break;
            }
        }

        internal static void InvokeHttpMethodAsync(this IInvocation invocation, ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            switch (req.HttpMethod)
            {
                case Attributes.HttpMethod.Get:
                    if (invocation.Method.ReturnType == typeof(Task))
                        invocation.ReturnValue = RestInterceptorUtils.GetAsync(serializer, headers, req);
                    else
                        invocation.ReturnValue = RestInterceptorUtils.GetAsync((dynamic)invocation.ReturnValue, serializer, headers, req);
                    break;
                case Attributes.HttpMethod.Post:
                    if (invocation.Method.ReturnType == typeof(Task))
                        invocation.ReturnValue = RestInterceptorUtils.PostAsync(serializer, headers, req);
                    else
                        invocation.ReturnValue = RestInterceptorUtils.PostAsync((dynamic)invocation.ReturnValue, serializer, headers, req);
                    break;
                default:
                    break;
            }
        }

    }

}
