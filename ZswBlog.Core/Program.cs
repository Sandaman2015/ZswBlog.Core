using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace ZswBlog.Core
{
    /// <summary>
    /// ��Ŀ������
    /// </summary>
    public class Program
    {
        /// <summary>
        /// ��������ڷ���
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {            
            CreateHostBuilder(args).Build().Run();
        }
        /// <summary>
        /// ���ó�ʼ��
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>().UseUrls("http://*:8004");
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());

    }
}
