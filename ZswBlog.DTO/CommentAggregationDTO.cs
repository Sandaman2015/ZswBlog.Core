using System.Collections.Generic;

namespace ZswBlog.DTO
{
    public partial class CommentAggregationDTO
    {
        /// <summary>
        /// 父级评论
        /// </summary>
        public CommentDTO CommentParent { get; set; }
        /// <summary>
        /// 子级评论
        /// </summary>
        public List<CommentDTO> CommentChildren { get; set; }
    }
}
