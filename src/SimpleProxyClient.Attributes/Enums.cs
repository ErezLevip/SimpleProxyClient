using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes
{
    public enum ArgSource
    {
        FromBody = 0,
        FromUrl,
        FromHeader
    }
    public enum InterceptorType
    {
        RestHttpInterceptor = 0
    }
    public enum HttpMethod
    {
        Get = 0,
        Post = 1
    }
}
