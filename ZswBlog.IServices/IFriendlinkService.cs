using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;

namespace ZswBlog.IServices
{
    public interface IFriendLinkService : IBaseService<FriendLinkEntity>
    {
        /// <summary>
        /// 获取所有的友情链接
        /// </summary>
        /// <returns></returns>
       Task<List<FriendLinkDTO>> GetAllFriendLinksAsync();
        /// <summary>
        /// 选择显示友情链接
        /// </summary>
        /// <param name="isShow">选择显示</param>
        /// <returns></returns>
        Task<List<FriendLinkDTO>> GetFriendLinksByIsShowAsync(bool isShow);
    }
}
