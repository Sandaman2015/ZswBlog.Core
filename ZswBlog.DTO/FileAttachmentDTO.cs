using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZswBlog.DTO
{
    /// <summary>
    /// 附件对象
    /// </summary>
    public class FileAttachmentDTO
    {
        /// <summary>
        /// 附件编码
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
        public string name { get; set; }
        /// <summary>
        /// 附件后缀
        /// </summary>
        public string fileExt { get; set; }
        /// <summary>
        /// 附件路径
        /// </summary>
        public string url { get; set; }

    }
}
