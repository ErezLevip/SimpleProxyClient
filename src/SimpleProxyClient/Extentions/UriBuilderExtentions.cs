using SimpleProxyClient.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace SimpleProxyClient.Extentions
{
    internal static class UriBuilderExtentions
    {
        internal static void AddQueryParameters(this UriBuilder builder, Dictionary<string, string> parameters)
        {
            var query = HttpUtility.ParseQueryString(string.Empty);

            foreach (var parameter in parameters)
            {
                query[parameter.Key] = parameter.Value;
            }

            builder.Query = query.ToString();
        }
    }
}
