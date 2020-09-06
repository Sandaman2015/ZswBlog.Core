using System.Collections.Generic;

namespace ZswBlog.DTO
{
    public partial class MessageAggregationDTO
    {
        /// <summary>
        /// 父级留言
        /// </summary>
        public MessageDTO MessageParent { get; set; }
        /// <summary>
        /// 子级留言
        /// </summary>
        public List<MessageDTO> MessagesChildren { get; set; }
    }
}
