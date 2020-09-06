using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITagService : IBaseService<Tag>
    {
        /// <summary>
        /// 获取所有标签信息
        /// </summary>
        /// <returns></returns>
        Task<List<Tag>> GetAllTagsAsync();
        /// <summary>
        /// 根据标签Id获取标签信息
        /// </summary>
        /// <param name="tagId"></param>
        /// <returns></returns>
        Task<Tag> GetTagByIdAsync(int tagId);
    }
}
