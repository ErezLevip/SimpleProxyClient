using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleProxyClient.Abstractions
{
    public interface ISerializer
    {
        object Deserialize(string str, Type type);
        string Serialize(object obj);
        T Deserialize<T>(string str);
    }
}
