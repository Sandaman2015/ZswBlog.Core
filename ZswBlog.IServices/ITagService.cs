using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITagService : IBaseService<TagEntity>
    {
        /// <summary>
        /// 获取所有标签信息
        /// </summary>
        /// <returns></returns>
        Task<List<TagEntity>> GetAllTagsAsync();
    }
}
