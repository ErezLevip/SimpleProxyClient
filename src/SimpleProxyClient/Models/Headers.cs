using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Models
{
    public class Headers : Dictionary<string,string>
    {
        public Headers() { }
        public Headers(Dictionary<string,string> dic)
        {
            foreach (var kvp in dic)
            {
                Add(kvp.Key, kvp.Value);
            }
        }
    }
}
