using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ITagService : IBaseService<TagEntity>
    {
        /// <summary>
        /// 获取所有标签信息
        /// </summary>
        /// <returns></returns>
        Task<List<TagDTO>> GetAllTagAsync();

        /// <summary>
        /// 根据id获取标签信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TagDTO> GetTagByIdAsync(int id);
    }
}
