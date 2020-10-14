using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Transactions;
using ZswBlog.Util;

namespace ZswBlog.Common.config
{
    /// <summary>
    /// AOP切面配置
    /// </summary>
    public class EnableTransactionScope : IInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
        public EnableTransactionScope()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            string className = invocation.Proxy.ToString().Replace("RepositoryProxy","仓储");
            string methodName = invocation.Method.Name;
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                NLogHelper.Default.Info(className+"类中"+methodName + "方法被调用了");
                invocation.Proceed();
                scope.Complete();
            }
            catch (Exception ex)
            {
                NLogHelper.Default.Error("记录错误日志：类名：" + className + "方法名：" + methodName + "错误信息：" + ex.Message);
            }
        }
    }
}