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
using Microsoft.Extensions.Logging;

namespace ZswBlog.Core
{
    /// <summary>
    /// ��Ŀ������
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ��ʼ��Configuration�ļ�
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// ���÷�������
        /// </summary>
        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "AllowAll";
        /// <summary>
        /// ��־����
        /// </summary>
        private static readonly ILoggerFactory _logFactory = LoggerFactory.Create(build =>
        {
            build.AddConsole();  // ���ڿ���̨��������
            build.AddDebug();    // ����VS���ԣ�������ڵ����
        });

        /// <summary>
        /// �м������ע��
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            //����
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
              builder => //builder.AllowAnyOrigin()
                         //�����Լ��������
              builder.WithOrigins("http://localhost:8080")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
            });


            //AutoMapperӳ���ļ�
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
            }));

            //���ȫ�ַ��ؽ�����쳣����������֤
            services.AddControllers(options =>
            {
                //options.Filters.Add<ValidateModelAttribute>();
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<BaseExceptionFilter>();
            });

            //����ת��
            services.AddMvc().AddJsonOptions(configure =>
             {
                 configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
             }).AddNewtonsoftJson(
                // 
                option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                );

            //Mysql���ӳ�
            var readConnection = Configuration.GetConnectionString("ClusterMysqlConnection");
            var writleConnection = Configuration.GetConnectionString("MasterMysqlConnection");
            services.AddDbContext<WritleDbContext>(options => options.UseMySql(writleConnection));
            services.AddDbContext<ReadDbContext>(options => options.UseMySql(readConnection)
            .EnableSensitiveDataLogging(true)
            .UseLoggerFactory(_logFactory)
            );


            //��ʼ�� RedisHelper
            var redisConnection = Configuration.GetConnectionString("RedisConnectionString");
            var csredis = new CSRedis.CSRedisClient(redisConnection);
            RedisHelper.Initialization(csredis);
            // ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                // ����ĵ���Ϣ
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
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
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
        /// ��������
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

            // ����Swagger�м��
            app.UseSwagger();

            // ����SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "ZswBlog ApiDocument");
            });

            //.net core webapi ����wwwroot�ļ��е����ã�������̬�ļ�
            app.UseStaticFiles();
            app.UseRouting();
            //��������
            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }
    }
}
