using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace ZswBlog.Core
{
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Program���� XML ע��
    public class Program
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Program���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Program.Main(string[])���� XML ע��
        public static void Main(string[] args)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Program.Main(string[])���� XML ע��
        {
            CreateHostBuilder(args).Build().Run();
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Program.CreateHostBuilder(string[])���� XML ע��
        public static IHostBuilder CreateHostBuilder(string[] args) =>
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Program.CreateHostBuilder(string[])���� XML ע��
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                }).UseServiceProviderFactory(new AutofacServiceProviderFactory());

    }
}
