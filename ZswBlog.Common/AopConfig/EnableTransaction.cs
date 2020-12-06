using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using System;
using System.Transactions;

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

        public ILogger logger{ get; set; }

        /// <summary>
        /// AOP开启事务控制
        /// </summary>
        /// <param name="invocation"></param>
        public void Intercept(IInvocation invocation)
        {
            string className = invocation.Proxy.ToString().Replace("Proxy", "").Replace("Castle.Proxies.", "");
            string methodName = invocation.Method.Name;
            try
            {
                using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                logger.LogInformation(className + "类中" + methodName + "方法开启事务提交");
                invocation.Proceed();
                scope.Complete();
                logger.LogInformation("事务提交成功");
            }
            catch (Exception ex)
            {
                logger.LogError("记录事务错误日志：类名：" + className + "，方法名：" + methodName + "，错误信息：" + ex.Message);
            }
        }
    }
}