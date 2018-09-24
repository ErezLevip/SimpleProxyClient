using SimpleProxyClient.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace SimpleProxyClient.Models
{
    internal class ReflectedData
    {
        public Attributes.HttpMethod HttpMethod { get; set; }
        public MethodInfo MethodInfo { get; set; }
        public InterceptorType InterceptorType { get; set; }
        public ControllerBinding ControllerBinding { get; set; }
        public MethodBinding MethodBinding { get; set; }
    }
}
