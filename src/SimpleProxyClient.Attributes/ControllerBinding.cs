using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyClient.Attributes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class ControllerBinding : Attribute
    {
        public string Controller { get; set; }
        public ControllerBinding(string controller)
        {
            Controller = controller;
        }
    }
}
