using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System.IO;

namespace ZswBlog.Core
{
    /// <summary>
    /// 项目启动类
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 主启动入口方法
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {            
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// 设置初始化
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:8004");
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory()).ConfigureLogging((hostingContext, logging) =>
                {
                    logging.ClearProviders(); //去掉默认添加的日志提供程序
                                              //添加控制台输出
                    logging.AddConsole();
                    //添加调试输出
                    //logging.AddDebug();
                });
            //.UseNLog();

    }
}
