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
using System.IO;
using ZswBlog.Admin.Profiles;
using ZswBlog.Entity;
using ZswBlog.Util;

namespace ZswBlog.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
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
                c.SwaggerDoc("v2", new OpenApiInfo
                {
                    Title = "ZswBlog Admin",
                    Version = "v2",
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
                c.IncludeXmlComments(xmlPath);
            });
            //Swagger 页面
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);
            //日期转换
            services.AddControllers(
                 setup =>
                 {
                     setup.ReturnHttpNotAcceptable = true;//不允许其他格式的请求
                 }
              ).AddJsonOptions(configure =>
              {
                  configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
              });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
            // 启用Swagger中间件
            app.UseSwagger();

            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "ZswBlog Admin ApiDocument");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //.net core webapi 访问wwwroot文件夹的配置，开启静态文件
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
