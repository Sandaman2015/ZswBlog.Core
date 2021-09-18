# ZswBlog.Core
## 此项目为ZswBlog后台开源代码
### 项目简介
该项目是集成了Zswblog前端主页和后台管理系统的后端项目

[ZswBlog前端](https://github.com/Sandaman2015/ZswBlog3.0)

[ZswBlog后台管理](https://github.com/Sandaman2015/ZswBlog.Manager)

### 开发环境：Visual Studio 2017 +

### 部署环境：Docker 

### 运行环境：.Net Core 3.1(Deprecated) -> .Net5.0

### 推荐数据库版本：Mysql8.0 +

### 技术选型：

- [x] Redis
- [x] Tencent QQ Login
- [x] EF Core 2.0
- [x] Mysql(support reading and writing separation)(支持读写分离)
- [x] Jwt Auth Token 
- [x] Swagger Documents
- [x] AutoFac(dependency injection) (依赖注入)
- [x] Exception Middleware Override (基于管道的异常组件)
- [x] AutoMapper(Object Mapping) 
- [x] Aliyun Oss (阿里云 OSS 对象存储)
- [x] Email Reply (邮箱回复)
- [x] Tencent Location Service (腾讯云位置服务)
- [x] Netease Music Cloud (网易云音乐接口)


### 补充说明：

Core项目的config中有如下文件：

**zswblog.sql（数据库脚本）**

**detailsPic.json（前端页面中文章详情页面的背景banner配置，支持手动或页面中替换）**

**indexVideo.json（前端页面中首页的视频或图片banner配置，支持手动或页面中替换）**

**illustration.json（分享，友链，分类，关于等前端页面中的Banner配置，支持手动或页面中替换）**

**visit.txt（浏览量统计，使用文件IO而非数据库存储的方式存储访问人数统计）**

Docker运行命令：

docker-compose-yml 内有注释，请先了解Docker以及Docker-Compose组件后再敲下面的命令

```
docker-compose up --build
```

本地运行先执行Nuget包还原，后启动ZswBlog.Core


