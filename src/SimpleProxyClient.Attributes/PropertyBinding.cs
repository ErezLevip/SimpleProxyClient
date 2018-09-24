using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes
{
    [AttributeUsage(AttributeTargets.Parameter)]
    public class PropertyBinding : Attribute
    {
        public ArgSource ArgSource { get; set; }
        public PropertyBinding(ArgSource argSource = ArgSource.FromBody)
        {
            ArgSource = argSource;
        }
    }
}
