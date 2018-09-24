using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace SimpleProxyClient.Extentions
{
    internal static class HttpClientExtentions
    {
        internal static void AddHeaders(this HttpClient httpClient, params Dictionary<string, string>[] headers)
        {
            if (headers != null)
            {
                foreach (var dictionary in headers)
                {
                    foreach (var kvp in dictionary)
                    {
                        if (httpClient.DefaultRequestHeaders.Contains(kvp.Key))
                            httpClient.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    }
                }
            }
        }
    }
}
