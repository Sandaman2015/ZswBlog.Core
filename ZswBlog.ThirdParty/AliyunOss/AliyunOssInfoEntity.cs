using Aliyun.OSS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace ZswBlog.ThirdParty.AliyunOss
{

    public class UploadPartContext
    {
        public string BucketName { get; set; }
        public string ObjectName { get; set; }

        public List<PartETag> PartETags { get; set; }

        public string UploadId { get; set; }
        public long TotalParts { get; set; }
        public long CompletedParts { get; set; }
        public object SyncLock { get; set; }
        public ManualResetEvent WaitEvent { get; set; }
    }

    public class UploadPartContextWrapper
    {
        public UploadPartContext Context { get; set; }
        public int PartNumber { get; set; }
        public Stream PartStream { get; set; }

        public UploadPartContextWrapper(UploadPartContext context, Stream partStream, int partNumber)
        {
            Context = context;
            PartStream = partStream;
            PartNumber = partNumber;
        }
    }

    public class UploadPartCopyContext
    {
        public string TargetBucket { get; set; }
        public string TargetObject { get; set; }

        public List<PartETag> PartETags { get; set; }

        public string UploadId { get; set; }
        public long TotalParts { get; set; }
        public long CompletedParts { get; set; }
        public object SyncLock { get; set; }
        public ManualResetEvent WaitEvent { get; set; }
    }

    public class UploadPartCopyContextWrapper
    {
        public UploadPartCopyContext Context { get; set; }
        public int PartNumber { get; set; }

        public UploadPartCopyContextWrapper(UploadPartCopyContext context, int partNumber)
        {
            Context = context;
            PartNumber = partNumber;
        }
    }
}
