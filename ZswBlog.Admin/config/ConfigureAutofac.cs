using Autofac;
using AutoMapper;
using System.Reflection;

namespace ZswBlog.Core.config
{
    /// <summary>
    /// Autofac的控制反转
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
            Assembly Repository = Assembly.Load("ZswBlog.Repository");
            //接口层所在程序集命名空间
            Assembly IRepository = Assembly.Load("ZswBlog.IRepository");
            //自动注入
            builder.RegisterAssemblyTypes(Repository, IRepository)
                .AsImplementedInterfaces().PropertiesAutowired().InstancePerDependency();

            Assembly services = Assembly.Load("ZswBlog.Services");
            Assembly IServices = Assembly.Load("ZswBlog.IServices");
            builder.RegisterAssemblyTypes(services, IServices)
                .AsImplementedInterfaces().PropertiesAutowired().InstancePerLifetimeScope();
            //AutoMapper的注入
            builder.RegisterType<Mapper>().As<IMapper>().AsSelf().PropertiesAutowired().InstancePerDependency();
            Assembly mappers = Assembly.Load("ZswBlog.MapperFactory");
            builder.RegisterAssemblyTypes(mappers)
                .AsSelf().PropertiesAutowired().SingleInstance();

            Assembly util = Assembly.Load("ZswBlog.Util");
            builder.RegisterAssemblyTypes(util)
                .AsSelf().PropertiesAutowired().SingleInstance();


            Assembly thirdParty = Assembly.Load("ZswBlog.ThirdParty");
            builder.RegisterAssemblyTypes(thirdParty)
                .AsSelf().PropertiesAutowired().SingleInstance();
            ////注册仓储，所有IRepository接口到Repository的映射
            //builder.RegisterGeneric(typeof(Repository))
            //    //InstancePerDependency：默认模式，每次调用，都会重新实例化对象；每次请求都创建一个新的对象；
            //    .As(typeof(IRepository)).InstancePerDependency();
        }
    }
}
