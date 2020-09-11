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
                // Ϊ Swagger JSON and UI����xml�ĵ�ע��·��
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//��ȡӦ�ó�������Ŀ¼�����ԣ����ܹ���Ŀ¼Ӱ�죬������ô˷�����ȡ·����
                var xmlPath = Path.Combine(basePath, "ZswBlog.Core.xml");
                c.IncludeXmlComments(xmlPath);
            });
            //Swagger ҳ��
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Latest);
            //����ת��
            services.AddControllers(
                 setup =>
                 {
                     setup.ReturnHttpNotAcceptable = true;//������������ʽ������
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
            // ����Swagger�м��
            app.UseSwagger();

            // ����SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v2/swagger.json", "ZswBlog Admin ApiDocument");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            //.net core webapi ����wwwroot�ļ��е����ã�������̬�ļ�
            app.UseStaticFiles();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
