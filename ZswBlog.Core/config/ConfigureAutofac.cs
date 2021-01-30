using Autofac;
using Autofac.Extras.DynamicProxy;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Reflection;
using ZswBlog.Common.AopConfig;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// AutoFacIOC配置类
    /// </summary>
    public class ConfigureAutofac : Autofac.Module
    {
        /// <summary>
        /// 控制反转
        /// </summary>
        /// <param name="builder"></param>
        protected override void Load(ContainerBuilder builder)
        {
            //业务逻辑层所在程序集命名空间
            var repositoryImpl = Assembly.Load("ZswBlog.Repository");
            //接口层所在程序集命名空间
            var repository = Assembly.Load("ZswBlog.IRepository");
            //自动注入
            builder.RegisterAssemblyTypes(repositoryImpl, repository)
                .AsImplementedInterfaces().PropertiesAutowired().InstancePerDependency();

            var servicesImpl = Assembly.Load("ZswBlog.Services");
            var services = Assembly.Load("ZswBlog.IServices");
            builder.RegisterAssemblyTypes(servicesImpl, services)
                .AsImplementedInterfaces().PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies)
                .InstancePerLifetimeScope();

            //AutoMapper的注入
            builder.RegisterType<Mapper>().As<IMapper>().AsSelf().PropertiesAutowired().InstancePerDependency();

            var util = Assembly.Load("ZswBlog.Common");
            builder.RegisterAssemblyTypes(util)
                .AsSelf().PropertiesAutowired().SingleInstance();

            var thirdParty = Assembly.Load("ZswBlog.ThirdParty");
            builder.RegisterAssemblyTypes(thirdParty)
                .AsSelf().PropertiesAutowired().SingleInstance();
            ////注册仓储，所有IRepository接口到Repository的映射
            //builder.RegisterGeneric(typeof(Repository))
            //    //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //    .As(typeof(IRepository)).InstancePerDependency();
            // builder.Register(c => new EnableTransaction());
        }
    }
}