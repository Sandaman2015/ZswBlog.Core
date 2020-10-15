using Castle.DynamicProxy;
using System;
using System.Transactions;
using ZswBlog.Util;

namespace ZswBlog.Common.AopConfig
{
    /// <summary>
    /// AOP切面配置
    /// </summary>
    public class EnableTransaction : IInterceptor
    {
        /// <summary>
        /// 
        /// </summary>
        public EnableTransaction()
        {
        }

        /// <summary>
        /// AOP开启事务控制
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            string className = invocation.Proxy.ToString().Replace("Proxy", "类");
            string methodName = invocation.Method.Name;
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                NLogHelper.Default.Info(className + "中" + methodName + "方法开启事务提交");
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