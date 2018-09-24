using SimpleProxyCore.Interceptors;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyCore.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PropertyBinding : Attribute
    {
        public ArgSource ArgSource { get; set; }
        public PropertyBinding(ArgSource argSource)
        {
            ArgSource = argSource;
        }
    }
}
