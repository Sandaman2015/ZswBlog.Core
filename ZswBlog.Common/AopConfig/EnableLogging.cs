using AspectCore.DynamicProxy;
using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZswBlog.Common.AopConfig
{
    public class EnableLogging : AbstractInterceptorAttribute
    {
        public async override Task Invoke(AspectContext context, AspectDelegate next)
        {
            Console.WriteLine("开始记录日志");            
            await next.Invoke(context);
            Console.WriteLine("结束记录日志");
        }
    }
}
