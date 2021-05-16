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
    /// ��Ŀ������
    /// </summary>
    public class Startup
    {
        private static readonly ILogger Logger = LoggerFactory.Create(build =>
        {
            build.AddConsole(); // ���ڿ���̨��������
            build.AddDebug(); // ����VS���ԣ�������ڵ����
        }).CreateLogger("Startup");

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
        private IConfiguration Configuration { get; }

        private const string MyAllowSpecificOrigins = "AllowAll";

        /// <summary>
        /// �˴���־�����������ݿ�ִ������
        /// </summary>
        private static readonly ILoggerFactory LogFactory = LoggerFactory.Create(build =>
        {
            build.ClearProviders(); //ȥ��Ĭ����ӵ���־�ṩ����
            build.AddDebug(); // ����VS���ԣ�������ڵ����
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
                        builder.WithOrigins("http://localhost:8080", "http://localhost:9528")
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .AllowCredentials());
            });

            //AutoMapperӳ���ļ�
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

            //���ȫ�ַ��ؽ�����쳣����������֤����־��¼
            services.AddControllers(options =>
            {
                options.Filters.Add<ApiResultFilter>();
                options.Filters.Add<LogFilter>();
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
            Logger.LogInformation($"��ȡ���ݿ��������ӵ�ַ��{readConnection}");
            Logger.LogInformation($"�������ݿ��������ӵ�ַ��{writleConnection}");
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

            //��ʼ�� RedisHelper
            var redisConnection = Configuration.GetConnectionString("RedisConnectionString");
            Logger.LogInformation($"Redis�������ӵ�ַ��{redisConnection}");
            var csRedis = new CSRedis.CSRedisClient(redisConnection);
            RedisHelper.Initialization(csRedis);
            // ע��Swagger����
            services.AddSwaggerGen(c =>
            {
                // ����ĵ���Ϣs
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
                var entityXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.Entity.xml");
                var dtoXmlPath = Path.Combine(AppContext.BaseDirectory, "ZswBlog.DTO.xml");
                c.IncludeXmlComments(coreXmlPath);
                c.IncludeXmlComments(entityXmlPath);
                c.IncludeXmlComments(dtoXmlPath);

                //Bearer ��scheme����
                var securityScheme = new OpenApiSecurityScheme()
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    //���������ͷ��
                    In = ParameterLocation.Header,
                    //ʹ��Authorizeͷ��
                    Type = SecuritySchemeType.Http,
                    //����Ϊ�� bearer��ͷ
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                };
                c.AddSecurityDefinition("bearerAuth", securityScheme);
            });
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);
            // jwt ��֤
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
                    //����ǩ����֤
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.SecretKey)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
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
                Logger.LogInformation("��ǰΪ����������Development");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                Logger.LogInformation("��ǰΪ����������Production");
            }

            //�Զ���HTTP״̬�������м��
            app.UseErrorHandling();

            //����Http�ض���
            app.UseHttpsRedirection();

            // ����Swagger�м��
            app.UseSwagger();

            // ����SwaggerUI
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v2/swagger.json", "ZswBlog ApiDocument"); });

            //����wwwroot�ļ��е����ã�������̬�ļ�
            app.UseStaticFiles();
            //����·��
            app.UseRouting();
            //��������
            app.UseCors(MyAllowSpecificOrigins);
            //����JWT��֤����
            app.UseAuthentication();
            app.UseAuthorization();
            //������ַӳ��
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        /// <summary>
        /// ����ע��
        /// </summary>
        /// <param name="containerBuilder"></param>
        public void ConfigureContainer(ContainerBuilder containerBuilder)
        {
            Logger.LogInformation("װ������ע������");
            containerBuilder.RegisterModule<ConfigureAutofac>();
        }
    }
}