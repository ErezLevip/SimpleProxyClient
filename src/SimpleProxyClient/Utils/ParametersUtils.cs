using SimpleProxyClient.Attributes;
using SimpleProxyClient.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleProxyClient.Utils
{
    internal static class ParametersUtils
    {
        internal static HttpRequestParameters ExtractHttpRequestParameters(object[] args, MethodInfo methodInfo)
        {
            HttpRequestParameters req = new HttpRequestParameters();
            int i = 0;
            foreach (var parameter in methodInfo.GetParameters())
            {
                var propBindingInfo = parameter.GetCustomAttribute<PropertyBinding>();
                if (propBindingInfo == null || propBindingInfo.ArgSource == ArgSource.FromBody)
                    req.BodyParameters.Add(parameter.Name, args[i]);
                else if (propBindingInfo.ArgSource == ArgSource.FromHeader)
                    req.HeadersParameters.Add(parameter.Name, args[i].ToString());
                else
                    req.QueryParameters.Add(parameter.Name, args[i].ToString());

                i++;
            }
            return req;
        }
    }
}
