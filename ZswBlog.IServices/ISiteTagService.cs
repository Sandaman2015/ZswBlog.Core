using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.IServices
{
    public interface ISiteTagService : IBaseService<SiteTagEntity>
    {
        /// <summary>
        /// 是否显示已审批的站点标签
        /// </summary>
        /// <param name="isShow"></param>
        /// <returns></returns>
        Task<List<SiteTagDTO>> GetSiteTagsByIsShowAsync(bool isShow);
        /// <summary>
        /// 获取所有站点标签
        /// </summary>
        /// <returns></returns>
        Task<List<SiteTagDTO>> GetAllSiteTagsAsync();
        /// <summary>
        /// 获取所有站点标签数量
        /// </summary>
        /// <returns></returns>
        Task<int> GetAllSiteTagsCountAsync();
        /// <summary>
        /// 删除站点标签
        /// </summary>
        /// <param name="tagEntity">删除实体</param>
        /// <returns></returns>
        Task<bool> RemoveSiteTagById(SiteTagEntity tagEntity);
    }
}
