using Castle.DynamicProxy;
using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Attributes;
using SimpleProxyClient.Extentions;
using SimpleProxyClient.Models;
using SimpleProxyClient.Utils;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleProxyClient.Interceptors
{
    [Interceptor(InterceptorType.RestHttpInterceptor)]
    public class RestInterceptor<T> : HttpInterceptorBase<T>, IProxyInterceptor, IHttpInterceptorFactory<T> where T : class
    {
        private TargetMethodInfo _targetMethodInfo;

        public void Proceed(IInvocation invocation)
        {
            var baseUri = new Uri(ServiceInformation.BaseUrl);
            var uriBuilder = new UriBuilder(new Uri(baseUri, _targetMethodInfo.Url));

            var parameters = ParametersUtils.ExtractHttpRequestParameters(invocation.Arguments, _targetMethodInfo.MethodInfo);
            // add query params
            uriBuilder.AddQueryParameters(parameters.QueryParameters);

            var headers = CombineParamHeadersWithGlobalHeaders(Headers, parameters.HeadersParameters);

            var url = uriBuilder.ToString();
            var req = new InvokeHttpRequest(url, parameters, _targetMethodInfo.HttpMethod);

            var invocationMethodReturnType = invocation.MethodInvocationTarget.ReturnType;
            if (IsTask(invocationMethodReturnType))
                invocation.InvokeHttpMethodAsync(Serializer, headers, req);
            else
                invocation.InvokeHttpMethod(Serializer, headers, req);
        }

        private static bool IsTask(Type invocationMethodReturnType)
        {
            return invocationMethodReturnType == typeof(Task) || invocationMethodReturnType.IsGenericType && invocationMethodReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }

        private Headers CombineParamHeadersWithGlobalHeaders(params Dictionary<string, string>[] dictionaries)
        {
            var headers = new Headers();
            foreach (var dic in dictionaries)
            {
                foreach (var kvp in dic)
                {
                    if (!headers.ContainsKey(kvp.Key))
                        headers.Add(kvp.Key, kvp.Value);
                }
            }
            return headers;
        }

        IProxyInterceptor IHttpInterceptorFactory<T>.Create(ISerializer serializer, ServiceInformation<T> serviceInfo, TargetMethodInfo targetMethodInfo, Headers headers)
        {
            return new RestInterceptor<T>
            {
                Headers = headers,
                Serializer = serializer,
                ServiceInformation = serviceInfo,
                _targetMethodInfo = targetMethodInfo
            };
        }
    }
}
