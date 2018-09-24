using SimpleProxyClient.Abstractions;
using SimpleProxyClient.Exceptions;
using SimpleProxyClient.Extentions;
using SimpleProxyClient.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SimpleProxyClient.Utils
{
    internal static class RestInterceptorUtils
    {
        internal static Task<T> GetAsync<T>(Task<T> originalTask, ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            return Task.Run(async () =>
            {
                using (HttpClient client = new HttpClient())
                {
                    var httpResponse = await PerformGetAsync(headers, req, client);
                    var jsonResponse = await httpResponse.Content.ReadAsStringAsync();
                    return ProcessResult<T>(serializer, jsonResponse);
                }
            });
        }

        private static async Task<HttpResponseMessage> PerformGetAsync(Headers headers, InvokeHttpRequest req, HttpClient client)
        {
            client.AddHeaders(headers);
            var httpResponse = await client.GetAsync(req.Url);
            if (!httpResponse.IsSuccessStatusCode)
            {
                throw new ProxyHttpException(httpResponse.StatusCode, httpResponse.ReasonPhrase);
            }
            return httpResponse;
        }

        internal static Task GetAsync(ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            return Task.Run(async () =>
            {
                using (HttpClient client = new HttpClient())
                {
                    await PerformGetAsync(headers, req, client);
                }
            });
        }

        internal static object Get(Type returnType, ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            using (HttpClient client = new HttpClient())
            {
                var httpResponse = PerformGetAsync(headers, req, client).Result;
                var jsonReponse = httpResponse.Content.ReadAsStringAsync().Result;
                return ProcessResult(serializer, jsonReponse, returnType);
            }
        }

        internal static void Get(ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            using (HttpClient client = new HttpClient())
            {
                PerformGetAsync(headers, req, client).GetAwaiter().GetResult();
            }
        }

        internal static object Post(Type returnType, ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            using (HttpClient client = new HttpClient())
            {
                var content = PerformPostAsync(serializer, headers, req, client).Result.Content;
                var jsonResponse = content.ReadAsStringAsync().Result;
                return ProcessResult(serializer, jsonResponse, returnType);
            }
        }

        private static async Task<HttpResponseMessage> PerformPostAsync(ISerializer serializer, Headers headers, InvokeHttpRequest req, HttpClient client)
        {
            client.AddHeaders(headers);
            var content = CreateJsonContent(serializer, req);
            var respose = await client.PostAsync(req.Url, content);

            if (!respose.IsSuccessStatusCode)
            {
                throw new ProxyHttpException(respose.StatusCode, respose.ReasonPhrase);
            }
            return respose;
        }

        internal static void Post(ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            using (HttpClient client = new HttpClient())
            {
                PerformPostAsync(serializer, headers, req, client).GetAwaiter().GetResult();
            }
        }

        internal static Task<T> PostAsync<T>(Task<T> originalTask, ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            return Task.Run(async () =>
            {
                using (HttpClient client = new HttpClient())
                {
                    var httpResponse = await PerformPostAsync(serializer, headers, req, client);

                    var content = await httpResponse.Content.ReadAsStringAsync();
                    return ProcessResult<T>(serializer, content);
                }
            });
        }

        internal static Task PostAsync(ISerializer serializer, Headers headers, InvokeHttpRequest req)
        {
            return Task.Run(async () =>
            {
                using (HttpClient client = new HttpClient())
                {
                    client.AddHeaders(headers);
                    StringContent requestContent = CreateJsonContent(serializer, req);
                    await client.PostAsync(req.Url, requestContent);
                }
            });
        }

        private static StringContent CreateJsonContent(ISerializer serializer, InvokeHttpRequest req)
        {
            if (req.Parameters.BodyParameters.Count == 1 &&
                req.Parameters.HeadersParameters.Count == 0 &&
                req.Parameters.QueryParameters.Count == 0)
            {
                return new StringContent(serializer.Serialize(req.Parameters.BodyParameters.First().Value), Encoding.UTF8, "application/json");
            }

            return new StringContent(serializer.Serialize(req.Parameters.BodyParameters), Encoding.UTF8, "application/json");
        }

        private static T ProcessResult<T>(ISerializer serializer, string responseContent)
        {
            var returnType = typeof(T);

            if (returnType.IsClass && returnType != typeof(string))
            {
                return serializer.Deserialize<T>(responseContent);
            }
            return (T)Convert.ChangeType(responseContent, typeof(T));
        }

        private static object ProcessResult(ISerializer serializer, string responseContent, Type type)
        {
            if (type.IsClass && type != typeof(string))
            {
                return serializer.Deserialize(responseContent, type);
            }
            return Convert.ChangeType(responseContent, type);
        }
    }
}
