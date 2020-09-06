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
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup���� XML ע��
    public class Startup
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup���� XML ע��
    {
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.Startup(IConfiguration)���� XML ע��
        public Startup(IConfiguration configuration)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.Startup(IConfiguration)���� XML ע��
        {
            Configuration = configuration;
        }

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.Configuration���� XML ע��
        public IConfiguration Configuration { get; }
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.Configuration���� XML ע��

        readonly string MyAllowSpecificOrigins = "AllowAll";

        // This method gets called by the runtime. Use this method to add services to the container.
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.ConfigureServices(IServiceCollection)���� XML ע��
        public void ConfigureServices(IServiceCollection services)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.ConfigureServices(IServiceCollection)���� XML ע��
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
                cfg.AddProfile<TagProfile>();
                cfg.AddProfile<MessageProfile>();
                cfg.AddProfile<CommentProfile>();
                cfg.AddProfile<FriendLinkProfile>();
                cfg.AddProfile<AboutProfile>();
                cfg.AddProfile<UserProfile>();
            }));

            //����ת��
            services.AddControllers(
                 setup =>
                 {
                     setup.ReturnHttpNotAcceptable = true;//������������ʽ������
                 }
              ).AddJsonOptions(configure =>
              {
                  configure.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
              }).AddNewtonsoftJson(option => option.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);

            //Mysql���ӳ�
            var connection = Configuration.GetConnectionString("MysqlConnection");
            IServiceCollection serviceCollections = services.AddDbContext<SingleBlogContext>(options => options.UseMySql(connection));


            //��ʼ�� RedisHelper
            var redisConnection = Configuration.GetConnectionString("RedisConnectionString");
            var csredis = new CSRedis.CSRedisClient(redisConnection);
            RedisHelper.Initialization(csredis);
            // ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                // ����ĵ���Ϣ
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
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var xmlPath = Path.Combine(basePath, "ZswBlog.Core.xml");
                c.IncludeXmlComments(xmlPath, true);
            });
#pragma warning disable CS0618 // '��CompatibilityVersion.Version_2_2���ѹ�ʱ:��This CompatibilityVersion value is obsolete. The recommended alternatives are Version_3_0 or later.��
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
#pragma warning restore CS0618 // '��CompatibilityVersion.Version_2_2���ѹ�ʱ:��This CompatibilityVersion value is obsolete. The recommended alternatives are Version_3_0 or later.��

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.Configure(IApplicationBuilder, IWebHostEnvironment)���� XML ע��
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.Configure(IApplicationBuilder, IWebHostEnvironment)���� XML ע��
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
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiDocument V1");
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

#pragma warning disable CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.ConfigureContainer(ContainerBuilder)���� XML ע��
        public void ConfigureContainer(ContainerBuilder containerBuilder)
#pragma warning restore CS1591 // ȱ�ٶԹ����ɼ����ͻ��Ա��Startup.ConfigureContainer(ContainerBuilder)���� XML ע��
        {
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }
    }
}
