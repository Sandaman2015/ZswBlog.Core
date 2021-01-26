using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Concurrent;
using System.Threading;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.Repository
{
    /// <summary>
    /// 单例实体工厂，在.Net Core中由于可以使用单例依赖注入则该工厂已经被抛弃
    /// </summary>    
    public class DbContextFactory
    {
        /// <summary>
        /// 创建实体
        /// </summary>
        [Obsolete("单例实体工厂，在.Net Core中由于可以使用单例依赖注入，则该创建实体对象工厂已经被抛弃")]
        public static DbContext Create()
        {
            //CallContext：是线程内部唯一的独用的数据槽（一块内存空间）类似于方法调用线程本地存储的专用的集合对象，
            //并提供专为执行的每个逻辑线程的数据槽。 槽不在其他逻辑线程的调用上下文之间共享。 
            //可以将对象添加到CallContext往返传播了执行代码路径，并且由各种沿着路径针对对象进行检查。
            //传递DbContext进去获取实例的信息，在这里进行强制转换。
            if (!(CallContext.GetData("DbContext") is DbContext dbContext))
            {
                //dbContext = new SingleBlogContext();
                dbContext = new ZswBlogDbContext();
                CallContext.SetData("DbContext", dbContext);
            }
            return dbContext;
        }
    }
    public static class CallContext//在dotnetCore 中没有CallContext类，所以只能自己手写
    {
        static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();

        public static void SetData(string name, object data) =>
            state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;

        public static object GetData(string name) =>
            state.TryGetValue(name, out AsyncLocal<object> data) ? data.Value : null;
    }
}
