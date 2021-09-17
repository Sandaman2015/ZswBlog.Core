using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

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

        /// <summary>
        /// 获取友情连接列表
        /// </summary>
        /// <returns></returns>
        Task<PageDTO<FriendLinkDTO>> GetFriendLinkListByPageAsync(int limit, int pageIndex);

        /// <summary>
        /// 更新友情连接
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        bool UpdateFriendLink(FriendLinkEntity entity);
        /// <summary>
        /// 删除友情连接
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        bool RemoveFriendLinkById(int tId);
        /// <summary>
        /// 获取友情链接详情
        /// </summary>
        /// <returns></returns>
        Task<FriendLinkDTO> GetFriendLinkByIdAsync(int tId);
    }
}
