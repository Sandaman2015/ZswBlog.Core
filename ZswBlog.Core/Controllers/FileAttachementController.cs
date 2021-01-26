using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using ZswBlog.Core.config;
using ZswBlog.Entity.DbContext;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.AliyunOss;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 附件控制器
    /// </summary>
    [ApiController]
    public class FileAttachmentController : ControllerBase
    {
        private readonly IFileAttachmentService _fileAttachmentService;
        private static string _bucketName;
        private static string _endPoint;

        /// <summary>
        /// 默认构造函数
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="fileAttachmentService"></param>
        public FileAttachmentController(IConfiguration configuration, IFileAttachmentService fileAttachmentService)
        {
            var configuration1 = configuration;
            _endPoint = configuration1.GetSection("endpoint").Value;
            _bucketName = configuration1.GetSection("bucketName").Value;
            _fileAttachmentService = fileAttachmentService;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        [Route("/api/attachment/upload/image")]
        [HttpPost]
        [Authorize]
        [FunctionDescription("上传文件")]
        public async Task<ActionResult<List<FileAttachmentEntity>>> UploadImageList(List<IFormFile> files,
            [FromQuery] int operatorId)
        {
            var fileAttachmentEntities = new List<FileAttachmentEntity>();
            var successCount = 0;
            foreach (var formFile in files)
            {
                string[] extendFileName =
                    {".png", ".jpg", ".jpeg", ".gif", ".bmp", ".BMP", ".JPEG", ".PNG", ".JPG", ".GIF"};
                var stream = formFile.OpenReadStream();
                var fileType = formFile.FileName.Substring(formFile.FileName.IndexOf('.'));
                var fileName =
                    EncryptProvider.Md5(formFile.FileName + DateTime.Now.ToString(CultureInfo.CurrentCulture));
                if (!extendFileName.Contains(fileType)) continue;
                //加密文件同赋予新路径
                var filepath = "attachment/" + fileName + fileType;
                var attachmentEntity = new FileAttachmentEntity()
                {
                    createDate = DateTime.Now,
                    fileExt = fileType,
                    operatorId = operatorId,
                    fileName = fileName,
                    //文件获取的路径返回
                    path = "https://" + _bucketName + "." + _endPoint + "/" + filepath
                };
                if (!await _fileAttachmentService.AddEntityAsync(attachmentEntity)) continue;
                if (!FileHelper.PushImg(stream, filepath)) continue;
                successCount++;
                fileAttachmentEntities.Add(attachmentEntity);
            }

            return Ok(new {count = successCount, result = fileAttachmentEntities});
        }

        /// <summary>
        /// 删除图片列表
        /// </summary>
        /// <returns></returns>
        [Route("/api/attachment/delete/image")]
        [HttpDelete]
        [Authorize]
        [FunctionDescription("删除图片列表")]
        public async Task<ActionResult<bool>> DeleteImageList([FromForm] string[] fileNames)
        {
            return await Task.Run(() =>
            {
                //简单模式批量删除文件
                var flag = FileHelper.DeleteObject(fileNames.ToList());
                dynamic rtValue = new {flag, msg = "删除成功"};
                return Ok(rtValue);
            });
        }
    }
}