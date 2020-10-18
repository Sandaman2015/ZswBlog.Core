using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IServices;
using ZswBlog.ThirdParty.AliyunOss;

namespace ZswBlog.Core.Controllers
{
    /// <summary>
    /// 附件控制器
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FileAttachmentController : ControllerBase
    {
        private readonly IFileAttachmentService _fileAttachmentService;
        private IConfiguration _configuration;
        private static string _bucketName;
        private static string _endPoint;

        public FileAttachmentController(IConfiguration configuration, IFileAttachmentService fileAttachmentService)
        {
            _configuration = configuration;
            _endPoint = _configuration.GetSection("endpoint").Value;
            _bucketName = _configuration.GetSection("bucketName").Value;
            _fileAttachmentService = fileAttachmentService;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="files"></param>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        [Route("/attachment/upload/image")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UploadImageList(List<IFormFile> files, [FromForm]int operatorId)
        {
            return await Task.Run(() =>
            {
                long size = files.Sum(f => f.Length);
                int successCount = 0;
                foreach (var formFile in files)
                {
                    string[] extendFileName = { ".png", ".jpg", ".jpeg", ".gif", ".bmp", ".BMP", ".JPEG", ".PNG", ".JPG", ".GIF" };
                    Stream stream = formFile.OpenReadStream();
                    string fileType = formFile.FileName.Substring(formFile.FileName.IndexOf('.'));
                    string fileName = EncryptProvider.Md5(formFile.FileName + DateTime.Now.ToString());
                    if (extendFileName.Contains(fileType))
                    {
                        //加密文件同赋予新路径
                        string filepath = "attachement/" + fileName + fileType;
                        FileAttachmentEntity attachmentEntity = new FileAttachmentEntity()
                        {
                            createDate = DateTime.Now,
                            fileExt = fileType,
                            operatorId = operatorId,
                            fileName = fileName,
                            //文件获取的路径返回
                            path = "https://" + _bucketName + "." + _endPoint + "/" + filepath
                        };
                        if (_fileAttachmentService.AddEntity(attachmentEntity))
                        {
                            if (FileUploadHelper.PushImg(stream, filepath)) successCount++;
                        }
                    }

                }
                return Ok(new { count = successCount, size });
            });
        }
    }
}
