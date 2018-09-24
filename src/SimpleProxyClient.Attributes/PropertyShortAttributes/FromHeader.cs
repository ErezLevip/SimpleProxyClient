using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes.PropertyShortAttributes
{
    public class FromHeader : PropertyBinding
    {
        public FromHeader() : base(ArgSource.FromHeader)
        {

        }
    }
}
