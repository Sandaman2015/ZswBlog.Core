using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    /// <summary>
    /// 附件类
    /// </summary>
    [Table("fileattachments")]
    public partial class FileAttachment
    {
        /// <summary>
        /// 附件id
        /// </summary>
        [Key]
        public Guid ID { get; set; }
        /// <summary>
        /// 上传路径
        /// </summary>
        public string Path { get; set; }
    }
}
