using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes.PropertyShortAttributes
{
    public class FromBody : PropertyBinding
    {
        public FromBody() : base(ArgSource.FromBody)
        {

        }
    }
}
