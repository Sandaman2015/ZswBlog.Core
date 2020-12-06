using Autofac;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using ZswBlog.Common.Profiles;
using System.IO;
using ZswBlog.Core.config;
using ZswBlog.Entity;
using ZswBlog.Common.Util;
using System;
using ZswBlog.Core.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ZswBlog.Common.Jwt;
using System.Reflection;
using ZswBlog.Common.AopConfig;

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
        /// 此处日志用来配置数据库执行语句的
        /// </summary>
        private static readonly ILoggerFactory _logFactory = LoggerFactory.Create(build =>
        {
            build.ClearProviders(); //去掉默认添加的日志提供程序
            build.AddDebug();    // 用于VS调试，输出窗口的输出
        });

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
              builder.WithOrigins("http://localhost:8080", "http://localhost:9528")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
            });

            //使用自带依赖注入日志配置到Aop事务
            services.AddSingleton((container) =>
            {
                return new EnableTransaction() { logger = _logFactory.CreateLogger("Transaction") };
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
                cfg.AddProfile<UserProfile>();
                cfg.AddProfile<AnnouncementProfile>();
                cfg.AddProfile<TravelProfile>();
                cfg.AddProfile<TagProfile>();
            }));

            //添加全局返回结果，异常处理，参数验证
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiResultFilter>();
            });

            //日期转换
            services.AddMvc().AddJsonOptions(configure =>
             {
                 configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
             }).AddNewtonsoftJson(
                // 
                option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );

            //Mysql连接池
            var readConnection = Configuration.GetConnectionString("ClusterMysqlConnection");
            var writleConnection = Configuration.GetConnectionString("MasterMysqlConnection");
            services.AddDbContext<WritleDbContext>(options => options.UseMySql(writleConnection));
            services.AddDbContext<ReadDbContext>(options => options.UseMySql(readConnection)
            .EnableSensitiveDataLogging(true)
            .UseLoggerFactory(_logFactory)
            );


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
                        Name = "Sandaman",
                        Email = "sandaman2015@163.com"
                    }
                });
                // 为 Swagger JSON and UI设置xml文档注释路径
                var coreXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.Core.xml");
                var entityXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.Entity.xml");
                var dTOXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.DTO.xml");
                c.IncludeXmlComments(coreXmlPath);
                c.IncludeXmlComments(entityXmlPath);
                c.IncludeXmlComments(dTOXmlPath);

                //Bearer 的scheme定义
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //参数添加在头部
                    In = ParameterLocation.Header,
                    //使用Authorize头部
                    Type = SecuritySchemeType.Http,
                    //内容为以 bearer开头
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                c.AddSecurityDefinition("bearerAuth", securityScheme);
            });
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);
            // jwt 认证
            JwtSettings jwtSettings = new JwtSettings();
            services.Configure<JwtSettings>(Configuration.GetSection("JwtSettings"));
            Configuration.GetSection("JwtSettings").Bind(jwtSettings);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    //用于签名验证
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
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

            //自定义HTTP状态错误反馈中间件
            app.UseErrorHandling();

            //开启Http重定向
            app.UseHttpsRedirection();

            // 启用Swagger中间件
            app.UseSwagger();

            // 配置SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "ZswBlog ApiDocument");
            });

            //访问wwwroot文件夹的配置，开启静态文件
            app.UseStaticFiles();
            //开启路由
            app.UseRouting();
            //跨域请求
            app.UseCors(MyAllowSpecificOrigins);
            //开启JWT认证服务
            app.UseAuthentication();
            app.UseAuthorization();
            //开启地址映射
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }
    }
}
