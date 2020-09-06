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
using SingleBlog.Web.Profiles;
using System.IO;
using ZswBlog.Core.config;
using ZswBlog.Entity;
using ZswBlog.Util;

namespace ZswBlog.Core
{
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Startup”的 XML 注释
    public class Startup
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Startup”的 XML 注释
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Startup.Startup(IConfiguration)”的 XML 注释
        public Startup(IConfiguration configuration)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Startup.Startup(IConfiguration)”的 XML 注释
        {
            Configuration = configuration;
        }

#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Startup.Configuration”的 XML 注释
        public IConfiguration Configuration { get; }
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Startup.Configuration”的 XML 注释

        readonly string MyAllowSpecificOrigins = "AllowAll";

        // This method gets called by the runtime. Use this method to add services to the container.
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Startup.ConfigureServices(IServiceCollection)”的 XML 注释
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Startup.ConfigureServices(IServiceCollection)”的 XML 注释
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
                cfg.AddProfile<TagProfile>();
                cfg.AddProfile<MessageProfile>();
                cfg.AddProfile<CommentProfile>();
                cfg.AddProfile<FriendLinkProfile>();
                cfg.AddProfile<AboutProfile>();
                cfg.AddProfile<UserProfile>();
            }));

            //日期转换
            services.AddControllers(
                 setup =>
                 {
                     setup.ReturnHttpNotAcceptable = true;//不允许其他格式的请求
                 }
              ).AddJsonOptions(configure =>
              {
                  configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
              }).AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //Mysql连接池
            var connection = Configuration.GetConnectionString("MysqlConnection");
            IServiceCollection serviceCollections = services.AddDbContext<SingleBlogContext>(options => options.UseMySql(connection));


            //初始化 RedisHelper
            var redisConnection = Configuration.GetConnectionString("RedisConnectionString");
            var csredis = new CSRedis.CSRedisClient(redisConnection);
            RedisHelper.Initialization(csredis);
            // 注册Swagger服务
            services.AddSwaggerGen(c =>
            {
                // 添加文档信息
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ZswBlog",
                    Version = "v1",
                    Description = "ZswBlog WebSite ASP.NET CORE WebApi",
                    Contact = new OpenApiContact
                    {
                        Name = "Sandman",
                        Email = "sandaman2015@163.com"
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
                var xmlPath = Path.Combine(basePath, "ZswBlog.Core.xml");
                c.IncludeXmlComments(xmlPath, true);
            });
#pragma warning disable CS0618 // '“CompatibilityVersion.Version_2_2”已过时:“This CompatibilityVersion value is obsolete. The recommended alternatives are Version_3_0 or later.”
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
#pragma warning restore CS0618 // '“CompatibilityVersion.Version_2_2”已过时:“This CompatibilityVersion value is obsolete. The recommended alternatives are Version_3_0 or later.”

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS1591 // 缺少对公共可见类型或成员“Startup.Configure(IApplicationBuilder, IWebHostEnvironment)”的 XML 注释
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CS1591 // 缺少对公共可见类型或成员“Startup.Configure(IApplicationBuilder, IWebHostEnvironment)”的 XML 注释
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDocument V1");
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
