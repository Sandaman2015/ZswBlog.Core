using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using ZswBlog.Common.Util;

namespace ZswBlog.ThirdParty.AliyunOss
{
    public class MultipartUploadHelper
    {

        /// <summary>
        /// 登录OSS控制台必须的key
        /// </summary>
        private static string accessKeyId;
        /// <summary>
        /// 登录OSS控制台必须的value
        /// </summary>
        private static string accessKeySecret;
        /// <summary>
        /// 域名,OSS控制台里能绑定自己的域名,没有自己的域名也无妨,默认是阿里云提供的域名地址
        /// </summary>
        private static string endpoint;
        /// <summary>
        /// OSS存储空间的名称
        /// </summary>
        private static string bucketName;

        static MultipartUploadHelper()
        {
            accessKeyId = ConfigHelper.GetValue("accessKeyId");
            accessKeySecret = ConfigHelper.GetValue("accessKeySecret");
            endpoint = ConfigHelper.GetValue("endpoint");
            bucketName = ConfigHelper.GetValue("bucketName");
        }
        /// <summary>
        /// 初始化阿里云存储空间对象
        /// </summary>
        static OssClient client = new OssClient(endpoint, accessKeyId, accessKeySecret);
        /// <summary>
        /// 设置分片上传的每一片的大小为50M
        /// </summary>
        static int partSize = 50 * 1024 * 1024;


        /// <summary>
        ///大文件分片上传
        /// </summary>
        /// <param name="bucketName">OSS存储空间名称</param>
        public static void UploadMultipart(String bucketName, String fileName, String localFilePath)
        {

            //初始化一个分片上传事件
            var uploadId = InitiateMultipartUpload(bucketName, fileName);
            var partETags = UploadParts(bucketName, fileName, localFilePath, uploadId, partSize);
            CompleteUploadPart(bucketName, fileName, uploadId, partETags);
            //Console.WriteLine("Multipart put object:{0} succeeded", key);
        }

        /// <summary>
        /// 异步分片上传。
        /// </summary>
        public static void AsyncUploadMultipart(String bucketName, String fileName, string localFilePath)
        {
            var uploadId = InitiateMultipartUpload(bucketName, fileName);
            AsyncUploadParts(bucketName, fileName, localFilePath, uploadId, partSize);
        }

        /// <summary>
        /// 分片拷贝。
        /// </summary>
        public static void UploadMultipartCopy(String targetBucket, String targetObject, String sourceBucket, String sourceObject)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetObject);
            var partETags = UploadPartCopys(targetBucket, targetObject, sourceBucket, sourceObject, uploadId, partSize);
            var completeResult = CompleteUploadPart(targetBucket, targetObject, uploadId, partETags);

            Console.WriteLine(@"Upload multipart copy result : ");
            Console.WriteLine(completeResult.Location);
        }

        /// <summary>
        /// 异步分片拷贝。
        /// </summary>
        public static void AsyncUploadMultipartCopy(String targetBucket, String targetObject, String sourceBucket, String sourceObject)
        {
            var uploadId = InitiateMultipartUpload(targetBucket, targetObject);
            AsyncUploadPartCopys(targetBucket, targetObject, sourceBucket, sourceObject, uploadId, partSize);
        }
        /// <summary>
        /// 列出所有执行中的Multipart Upload事件
        /// </summary>
        /// <param name="bucketName">目标bucket名称</param>
        public static void ListMultipartUploads(String bucketName)
        {
            var listMultipartUploadsRequest = new ListMultipartUploadsRequest(bucketName);
            var result = client.ListMultipartUploads(listMultipartUploadsRequest);
            Console.WriteLine("Bucket name:" + result.BucketName);
            Console.WriteLine("Key marker:" + result.KeyMarker);
            Console.WriteLine("Delimiter:" + result.Delimiter);
            Console.WriteLine("Prefix:" + result.Prefix);
            Console.WriteLine("UploadIdMarker:" + result.UploadIdMarker);

            foreach (var part in result.MultipartUploads)
            {
                Console.WriteLine(part.ToString());
            }
        }


        /// <summary>
        /// 初始化一个分片上传事件
        /// </summary>
        /// <param name="bucketName">存储空间名称</param>
        /// <param name="objectName"></param>
        /// <returns></returns>
        public static string InitiateMultipartUpload(String bucketName, String objectName)
        {
            var request = new InitiateMultipartUploadRequest(bucketName, objectName);
            var result = client.InitiateMultipartUpload(request);
            return result.UploadId;
        }
        /// <summary>
        /// 上传分片
        /// </summary>
        /// <param name="bucketName">OSS存储空间名称</param>
        /// <param name="objectName">上传到OSS后的文件名称</param>
        /// <param name="fileToUpload">本地待上传的文件</param>
        /// <param name="uploadId">上传编号</param>
        /// <param name="partSize">分片大小</param>
        /// <returns>分片集合</returns>
        public static List<PartETag> UploadParts(String bucketName, String objectName, String fileToUpload,
                                                  String uploadId, int partSize)
        {
            var fi = new FileInfo(fileToUpload);
            var fileSize = fi.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            using (var fs = File.Open(fileToUpload, FileMode.Open))
            {
                for (var i = 0; i < partCount; i++)
                {
                    var skipBytes = (long)partSize * i;
                    fs.Seek(skipBytes, 0);
                    var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                    var request = new UploadPartRequest(bucketName, objectName, uploadId)
                    {
                        InputStream = fs,
                        PartSize = size,
                        PartNumber = i + 1
                    };

                    var result = client.UploadPart(request);

                    partETags.Add(result.PartETag);
                    Console.WriteLine("finish {0}/{1}", partETags.Count, partCount);
                }
            }
            return partETags;
        }

        public static void AsyncUploadParts(String bucketName, String objectName, String fileToUpload,
            String uploadId, int partSize)
        {
            var fi = new FileInfo(fileToUpload);
            var fileSize = fi.Length;
            var partCount = fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var ctx = new UploadPartContext()
            {
                BucketName = bucketName,
                ObjectName = objectName,
                UploadId = uploadId,
                TotalParts = partCount,
                CompletedParts = 0,
                SyncLock = new object(),
                PartETags = new List<PartETag>(),
                WaitEvent = new ManualResetEvent(false)
            };

            for (var i = 0; i < partCount; i++)
            {
                var fs = new FileStream(fileToUpload, FileMode.Open, FileAccess.Read, FileShare.Read);
                var skipBytes = (long)partSize * i;
                fs.Seek(skipBytes, 0);
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request = new UploadPartRequest(bucketName, objectName, uploadId)
                {
                    InputStream = fs,
                    PartSize = size,
                    PartNumber = i + 1
                };
                client.BeginUploadPart(request, UploadPartCallback, new UploadPartContextWrapper(ctx, fs, i + 1));
            }

            ctx.WaitEvent.WaitOne();
        }

        public static void UploadPartCallback(IAsyncResult ar)
        {
            var result = client.EndUploadPart(ar);
            var wrappedContext = (UploadPartContextWrapper)ar.AsyncState;
            wrappedContext.PartStream.Close();

            var ctx = wrappedContext.Context;
            lock (ctx.SyncLock)
            {
                var partETags = ctx.PartETags;
                partETags.Add(new PartETag(wrappedContext.PartNumber, result.ETag));
                ctx.CompletedParts++;

                Console.WriteLine("finish {0}/{1}", ctx.CompletedParts, ctx.TotalParts);
                if (ctx.CompletedParts == ctx.TotalParts)
                {
                    partETags.Sort((e1, e2) => (e1.PartNumber - e2.PartNumber));
                    var completeMultipartUploadRequest =
                        new CompleteMultipartUploadRequest(ctx.BucketName, ctx.ObjectName, ctx.UploadId);
                    foreach (var partETag in partETags)
                    {
                        completeMultipartUploadRequest.PartETags.Add(partETag);
                    }

                    var completeMultipartUploadResult = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                    Console.WriteLine(@"Async upload multipart result : " + completeMultipartUploadResult.Location);

                    ctx.WaitEvent.Set();
                }
            }
        }

        public static List<PartETag> UploadPartCopys(String targetBucket, String targetObject, String sourceBucket, String sourceObject,
            String uploadId, int partSize)
        {
            var metadata = client.GetObjectMetadata(sourceBucket, sourceObject);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var partETags = new List<PartETag>();
            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request =
                    new UploadPartCopyRequest(targetBucket, targetObject, sourceBucket, sourceObject, uploadId)
                    {
                        PartSize = size,
                        PartNumber = i + 1,
                        BeginIndex = skipBytes
                    };
                var result = client.UploadPartCopy(request);
                partETags.Add(result.PartETag);
            }

            return partETags;
        }

        public static void AsyncUploadPartCopys(String targetBucket, String targetObject, String sourceBucket, String sourceObject,
            String uploadId, int partSize)
        {
            var metadata = client.GetObjectMetadata(sourceBucket, sourceObject);
            var fileSize = metadata.ContentLength;

            var partCount = (int)fileSize / partSize;
            if (fileSize % partSize != 0)
            {
                partCount++;
            }

            var ctx = new UploadPartCopyContext()
            {
                TargetBucket = targetBucket,
                TargetObject = targetObject,
                UploadId = uploadId,
                TotalParts = partCount,
                CompletedParts = 0,
                SyncLock = new object(),
                PartETags = new List<PartETag>(),
                WaitEvent = new ManualResetEvent(false)
            };

            for (var i = 0; i < partCount; i++)
            {
                var skipBytes = (long)partSize * i;
                var size = (partSize < fileSize - skipBytes) ? partSize : (fileSize - skipBytes);
                var request =
                    new UploadPartCopyRequest(targetBucket, targetObject, sourceBucket, sourceObject, uploadId)
                    {
                        PartSize = size,
                        PartNumber = i + 1,
                        BeginIndex = skipBytes
                    };
                client.BeginUploadPartCopy(request, UploadPartCopyCallback, new UploadPartCopyContextWrapper(ctx, i + 1));
            }

            ctx.WaitEvent.WaitOne();
        }

        public static void UploadPartCopyCallback(IAsyncResult ar)
        {
            var result = client.EndUploadPartCopy(ar);
            var wrappedContext = (UploadPartCopyContextWrapper)ar.AsyncState;

            var ctx = wrappedContext.Context;
            lock (ctx.SyncLock)
            {
                var partETags = ctx.PartETags;
                partETags.Add(new PartETag(wrappedContext.PartNumber, result.ETag));
                ctx.CompletedParts++;

                if (ctx.CompletedParts == ctx.TotalParts)
                {
                    partETags.Sort((e1, e2) => (e1.PartNumber - e2.PartNumber));
                    var completeMultipartUploadRequest =
                        new CompleteMultipartUploadRequest(ctx.TargetBucket, ctx.TargetObject, ctx.UploadId);
                    foreach (var partETag in partETags)
                    {
                        completeMultipartUploadRequest.PartETags.Add(partETag);
                    }

                    var completeMultipartUploadResult = client.CompleteMultipartUpload(completeMultipartUploadRequest);
                    Console.WriteLine(@"Async upload multipart copy result : " + completeMultipartUploadResult.Location);

                    ctx.WaitEvent.Set();
                }
            }
        }
        /// <summary>
        /// 完成分片上传
        /// </summary>
        /// <param name="bucketName">OSS存储空间名称</param>
        /// <param name="objectName">OSS存储空间上的文件名称</param>
        /// <param name="uploadId">上传编号</param>
        /// <param name="partETags">分片集合</param>
        /// <returns></returns>
        public static CompleteMultipartUploadResult CompleteUploadPart(String bucketName, String objectName, String uploadId, List<PartETag> partETags)
        {
            var completeMultipartUploadRequest =
                new CompleteMultipartUploadRequest(bucketName, objectName, uploadId);
            foreach (var partETag in partETags)
            {
                completeMultipartUploadRequest.PartETags.Add(partETag);
            }

            return client.CompleteMultipartUpload(completeMultipartUploadRequest);
        }


        /// <summary>
        /// 上传一个新文件
        /// </summary>
        public static void PutObject(string _key, string _fileToUpload)
        {
            try
            {
                client.PutObject(bucketName, _key, _fileToUpload);
                // Console.WriteLine("上传文件成功");
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                //Console.WriteLine("上传文件失败.原因: {0}", ex.Message);
            }
        }
        /// <summary>
        /// 上传一个图片
        /// </summary>
        /// <param name="base64Code">图片经过base64加密后的结果</param>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        public bool PushImg(string base64Code, string fileName)
        {
            try
            {
                MemoryStream stream = new MemoryStream(Convert.FromBase64String(base64Code));
                return client.PutObject(bucketName, fileName, stream).HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            { }
            return false;
        }
        /// <summary>
        /// 上传一个文件
        /// </summary>
        /// <param name="filebyte">图片字节 </param>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        public bool PushFile(byte[] filebyte, string fileName)
        {
            try
            {
                MemoryStream stream = new MemoryStream(filebyte, 0, filebyte.Length);
                return client.PutObject(bucketName, fileName, stream).HttpStatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            { }
            return false;
        }
        /// <summary>
        /// 获取鉴权后的URL,文件过期时间默认设置为100年
        /// </summary>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        /// <returns></returns>
        public string GetFileUrl(string fileName)
        {
            var key = fileName;
            var req = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get)
            {
                Expiration = DateTime.Now.AddYears(100)
            };
            return client.GeneratePresignedUri(req).ToString();
        }
        /// <summary>
        /// 获取鉴权后的URL
        /// </summary>
        /// <param name="fileName">文件名,例如:Emplyoee/dzzBack.jpg</param>
        /// <param name="expiration">URL有效日期,例如:DateTime.Now.AddHours(1) </param>
        /// <returns></returns>
        public string GetFileUrl(string fileName, DateTime expiration)
        {
            var key = fileName;
            var req = new GeneratePresignedUriRequest(bucketName, key, SignHttpMethod.Get)
            {
                Expiration = expiration
            };
            return client.GeneratePresignedUri(req).ToString();
        }
    }
}
