using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Models
{
    public class ServiceInformation<T>
    {
        public string BaseUrl { get; set; }
        public TimeSpan Ttl { get; set; }
    }
}
