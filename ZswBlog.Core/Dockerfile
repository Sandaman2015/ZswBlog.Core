#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 8004

#配置创建人
MAINTAINER Sandaman2015@163.com
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["ZswBlog.Core/ZswBlog.Core.csproj", "ZswBlog.Core/"]
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
#设置时间为中国上海
ENV TZ=Asia/Shanghai
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone
ENTRYPOINT ["dotnet", "ZswBlog.Core.dll"]