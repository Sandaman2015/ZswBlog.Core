using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ZswBlog.Entity
{
    [Table("fileattachments")]
    public partial class FileAttachment
    {
        [Key]
        public Guid ID { get; set; }
        public string Path { get; set; }
    }
}
