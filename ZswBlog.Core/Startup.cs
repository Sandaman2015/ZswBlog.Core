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
using ZswBlog.Entity.DbContext;
using ZswBlog.Common.Util;
using System;
using ZswBlog.Core.Controllers;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Pomelo.EntityFrameworkCore.MySql.Storage;

namespace ZswBlog.Core
{
    /// <summary>
    /// 项目启动类
    /// </summary>
    public class Startup
    {
        private static readonly ILogger Logger = LoggerFactory.Create(build =>
        {
            build.AddConsole(); // 用于控制台程序的输出
            build.AddDebug(); // 用于VS调试，输出窗口的输出
        }).CreateLogger("Startup");

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
        private IConfiguration Configuration { get; }

        private const string MyAllowSpecificOrigins = "AllowAll";

        /// <summary>
        /// 此处日志用来配置数据库执行语句的
        /// </summary>
        private static readonly ILoggerFactory LogFactory = LoggerFactory.Create(build =>
        {
            build.ClearProviders(); //去掉默认添加的日志提供程序
            build.AddDebug(); // 用于VS调试，输出窗口的输出
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

            //AutoMapper映射文件
            services.AddSingleton((AutoMapper.IConfigurationProvider) new MapperConfiguration(cfg =>
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
                cfg.AddProfile<FileAttachmentProfile>();
            }));

            //添加全局返回结果，异常处理，参数验证，日志记录
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<LogFilter>();
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
            Logger.LogInformation($"读取数据库配置连接地址：{readConnection}");
            Logger.LogInformation($"更新数据库配置连接地址：{writleConnection}");
            //ServerVersion serverVersion1 = ServerVersion.AutoDetect(writleConnection);
            //services.AddDbContext<WritleDbContext>(options => options.UseMySql(writleConnection, serverVersion1)
            //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).UseLoggerFactory(LogFactory), ServiceLifetime.Scoped);

            //ServerVersion serverVersion2 = ServerVersion.AutoDetect(readConnection);
            //services.AddDbContext<ReadDbContext>(options => options.UseMySql(readConnection, serverVersion2)
            //.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).UseLoggerFactory(LogFactory), ServiceLifetime.Transient);

            ServerVersion serverVersion3 = ServerVersion.AutoDetect(readConnection);
            services.AddDbContext<ZswBlogDbContext>(options => options.UseMySql(readConnection, serverVersion3,
                 builder =>
                 {
                     builder.EnableRetryOnFailure(
                         maxRetryCount: 5,
                         maxRetryDelay: TimeSpan.FromSeconds(30),
                         errorNumbersToAdd: null);
                 })
            .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking).UseLoggerFactory(LogFactory)).AddTransient<ZswBlogDbContext>();

            //初始化 RedisHelper
            var redisConnection = Configuration.GetConnectionString("RedisConnectionString");
            Logger.LogInformation($"Redis配置连接地址：{redisConnection}");
            var csRedis = new CSRedis.CSRedisClient(redisConnection);
            RedisHelper.Initialization(csRedis);
            // 注册Swagger服务
            services.AddSwaggerGen(c =>
            {
                // 添加文档信息s
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
                var entityXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.Entity.xml");
                var dtoXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.DTO.xml");
                c.IncludeXmlComments(coreXmlPath);
                c.IncludeXmlComments(entityXmlPath);
                c.IncludeXmlComments(dtoXmlPath);

                //Bearer 的scheme定义
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
            var jwtSettings = new JwtSettings();
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
                Logger.LogInformation("当前为开发环境：Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                Logger.LogInformation("当前为生产环境：Production");
            }

            //自定义HTTP状态错误反馈中间件
            app.UseErrorHandling();

            //开启Http重定向
            app.UseHttpsRedirection();

            // 启用Swagger中间件
            app.UseSwagger();

            // 配置SwaggerUI
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v2/swagger.json", "ZswBlog ApiDocument"); });

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
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            Logger.LogInformation("装载依赖注入配置");
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }
    }
}