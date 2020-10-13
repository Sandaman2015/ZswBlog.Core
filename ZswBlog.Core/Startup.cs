using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ZswBlog.Core.Profiles;
using System.IO;
using ZswBlog.Core.config;
using ZswBlog.Entity;
using ZswBlog.Util;
using System;
using ZswBlog.Core.Controllers;

namespace ZswBlog.Core
{
    /// <summary>
    /// 项目启动类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 初始化Configuration文件
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// 配置访问属性
        /// </summary>
        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "AllowAll";

        /// <summary>
        /// 中间件服务注册
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //跨域
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
              builder => //builder.AllowAnyOrigin()
                         //根据自己情况调整
              builder.WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
            });


            //AutoMapper映射文件
            services.AddSingleton((AutoMapper.IConfigurationProvider)new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ArticleProfile>();
                cfg.AddProfile<MessageProfile>();
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<CommentProfile>();
                cfg.AddProfile<CategoryProfile>();
                cfg.AddProfile<SiteTagProfile>();
                cfg.AddProfile<TimeLineProfile>();
                cfg.AddProfile<FriendLinkProfile>();
                
            }));

            //添加全局返回结果，异常处理，参数验证
            services.AddControllers(options =>
            {
                //options.Filters.Add<ValidateModelAttribute>();
                options.Filters.Add<ApiResultFilterAttribute>();
                options.Filters.Add<BaseExceptionAttribute>();
            });

            //日期转换
            services.AddMvc().AddJsonOptions(configure =>
             {
                 configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
             }).AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //Mysql连接池
            var connection = Configuration.GetConnectionString("MysqlConnection");
            IServiceCollection serviceCollections = services.AddDbContext<ZswBlogDbContext>(options => options.UseMySql(connection));


            //初始化 RedisHelper
            var redisConnection = Configuration.GetConnectionString("RedisConnectionString");
            var csredis = new CSRedis.CSRedisClient(redisConnection);
            RedisHelper.Initialization(csredis);
            // 注册Swagger服务
            services.AddSwaggerGen(c =>
            {
                // 添加文档信息
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "ZswBlog",
                    Version = "v2",
                    Description = "ZswBlog WebSite ASP.NET CORE WebApi",
                    Contact = new OpenApiContact
                    {
                        Name = "Sandman",
                        Email = "sandaman2015@163.com"
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var coreXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.Core.xml");
                var EntityXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.Entity.xml");
                var DTOXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.DTO.xml");
                c.IncludeXmlComments(coreXmlPath);
                c.IncludeXmlComments(EntityXmlPath);
                c.IncludeXmlComments(DTOXmlPath);
            });
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);
        }

        /// <summary>
        /// 设置配置
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(appBuilder =>
                {
                    appBuilder.Run(async context =>
                    {
                        var feature = context.Features.Get<IExceptionHandlerPathFeature>();
                        var exception = feature.Error;
                        var result = JsonConvert.SerializeObject(new { error = exception.Message });
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(result);
                    });
                });
            }


            app.UseHttpsRedirection();

            // 启用Swagger中间件
            app.UseSwagger();

            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "ZswBlog ApiDocument");
            });

            //.net core webapi 访问wwwroot文件夹的配置，开启静态文件
            app.UseStaticFiles();
            app.UseRouting();
            //跨域请求
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Startup.ConfigureContainer(ContainerBuilder)”的 XML 注释
        public void ConfigureContainer(ContainerBuilder containerBuilder)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Startup.ConfigureContainer(ContainerBuilder)”的 XML 注释
        {
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }
    }
}
