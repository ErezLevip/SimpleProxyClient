using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleProxyCore.Attributes
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
