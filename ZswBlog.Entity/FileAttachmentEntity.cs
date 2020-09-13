﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 上传附件实体对象
    /// </summary>
    public class FileAttachmentEntity
    {
        /// <summary>
        /// 附件id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 附件上传时间
        /// </summary>
        public DateTime createDate { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public int operatorId { get; set; }
        /// <summary>
        /// 附件名称
        /// </summary>
        public string fileName { get; set; }
        /// <summary>
        /// 附件后缀
        /// </summary>
        public string fileExt { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string path { get; set; }
    }
}