using Castle.DynamicProxy;
using SimpleProxyClient.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tester
{
    public class ExceptionInterceptor : IProxyInterceptor
    {
        public IProxyInterceptor Next { get; set; }

        public void Proceed(IInvocation invocation)
        {
            try
            {
                Next.Proceed(invocation);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
