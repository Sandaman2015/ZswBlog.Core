using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Common.AopConfig
{
    public class EnableLogging : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }
}
