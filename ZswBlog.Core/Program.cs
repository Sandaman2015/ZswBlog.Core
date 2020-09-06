using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ZswBlog.Core
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Program”的 XML 注释
    public class Program
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Program”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Program.Main(string[])”的 XML 注释
        public static void Main(string[] args)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Program.Main(string[])”的 XML 注释
        {
            CreateHostBuilder(args).Build().Run();
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Program.CreateHostBuilder(string[])”的 XML 注释
        public static IHostBuilder CreateHostBuilder(string[] args) =>
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Program.CreateHostBuilder(string[])”的 XML 注释
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());

    }
}
