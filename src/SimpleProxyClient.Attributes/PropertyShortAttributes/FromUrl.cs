using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes.PropertyShortAttributes
{
    public class FromUrl : PropertyBinding
    {
        public FromUrl() : base(ArgSource.FromUrl)
        {

        }
    }
}
