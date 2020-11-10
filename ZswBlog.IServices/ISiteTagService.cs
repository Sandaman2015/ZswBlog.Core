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
        List<SiteTagDTO> GetSiteTagsByIsShow(bool isShow);
        /// <summary>
        /// 获取所有站点标签
        /// </summary>
        /// <returns></returns>
        List<SiteTagDTO> GetAllSiteTags();
        /// <summary>
        /// 获取所有站点标签数量
        /// </summary>
        /// <returns></returns>
        int GetAllSiteTagsCount();
    }
}
