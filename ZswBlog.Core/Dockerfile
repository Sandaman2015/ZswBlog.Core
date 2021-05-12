#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

#配置创建人
MAINTAINER Sandaman2015@163.com
FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["ZswBlog.Core/ZswBlog.Core.csproj", "ZswBlog.Core/"]
COPY ["ZswBlog.Core/ZswBlog.Core/config", "ZswBlog.Core/config"]
COPY ["ZswBlog.Query/ZswBlog.Query.csproj", "ZswBlog.Query/"]
COPY ["ZswBlog.IRepository/ZswBlog.IRepository.csproj", "ZswBlog.IRepository/"]
COPY ["ZswBlog.DTO/ZswBlog.DTO.csproj", "ZswBlog.DTO/"]
COPY ["ZswBlog.Entity/ZswBlog.Entity.csproj", "ZswBlog.Entity/"]
COPY ["ZswBlog.Common/ZswBlog.Common.csproj", "ZswBlog.Common/"]
COPY ["ZswBlog.Services/ZswBlog.Services.csproj", "ZswBlog.Services/"]
COPY ["ZswBlog.ThirdParty/ZswBlog.ThirdParty.csproj", "ZswBlog.ThirdParty/"]
COPY ["ZswBlog.IServices/ZswBlog.IServices.csproj", "ZswBlog.IServices/"]
COPY ["ZswBlog.Repository/ZswBlog.Repository.csproj", "ZswBlog.Repository/"]
RUN dotnet restore "ZswBlog.Core/ZswBlog.Core.csproj"
COPY . .
WORKDIR "/src/ZswBlog.Core"
RUN dotnet build "ZswBlog.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ZswBlog.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
#配置默认启动地址
ENV ASPNETCORE_URLS=http://+:8004
#配置为开发环境
ENV ASPNETCORE_ENVIRONMENT=Development
#配置Appsetting.json的配置
ENV CONNECTIONSTRINGS_REDISCONNECTIONSTRING="182.61.148.221:6379,password=732668,database=0"
ENV CONNECTIONSTRINGS_CLUSTERMYSQLCONNECTION="server=182.61.148.221;port=3306;database=zswblog;SslMode=None;uid=root;pwd=732668;Allow User Variables=true;"
ENV CONNECTIONSTRINGS_MASTERMYSQLCONNECTION="server=182.61.148.221;port=3306;database=zswblog;SslMode=None;uid=root;pwd=732668;Allow User Variables=true;"
ENV QQAPPKEY=101858486
ENV QQAPPSECRET=9ae0e34cadf786a0a7050659423a55e8
ENV LOCATIONKEY=HIQBZ-WNQKK-6VZJ7-AR3XK-HEVAK-5DBXD
ENV EMAILSENDADDRESS=1761317983@qq.com
ENV EMAILSECRETKEY=emqwtiqpmrpccbae
ENV MUSICBASESITE=/PLAYLIST/DETAIL?ID=585511463
ENV MUSICBASEURL=http://121.36.93.244:3000
#阿里云
ENV ACCESSKEYID=LTAI4GDdBoYhq6z8MHd9cxim
ENV ACCESSKEYSECRET=31p2Z0XTVFvhrhl1e1JhzqAV5lYBL4
ENV ENDPOINT=oss-cn-hangzhou.aliyuncs.com
ENV BUCKETNAME=zswblog
#设置时间为中国上海
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT ["dotnet", "ZswBlog.Core.dll"]