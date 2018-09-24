using Newtonsoft.Json;
using SimpleProxyClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using SimpleProxyClient.Attributes;

namespace SimpleProxyClient.Serializers
{
    public class JsonNetSerializer : ISerializer
    {
        public T Deserialize<T>(string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        public object Deserialize(string str, Type type)
        {
            return JsonConvert.DeserializeObject(str, type);
        }

        public string Serialize(object obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
    }

}
