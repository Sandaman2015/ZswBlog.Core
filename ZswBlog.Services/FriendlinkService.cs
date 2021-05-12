using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class FriendLinkService : BaseService<FriendLinkEntity, IFriendLinkRepository>, IFriendLinkService
    {
        public IFriendLinkRepository FriendLinkRepository { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 获取所有友情链接
        /// </summary>
        /// <returns></returns>
        public async Task<List<FriendLinkDTO>> GetAllFriendLinksAsync()
        {
            return await Task.Run(() =>
            {
                var friendLinks = FriendLinkRepository.GetModels(a => a.id != 0);
                return Mapper.Map<List<FriendLinkDTO>>(friendLinks.ToList());
            });
        }

        /// <summary>
        /// 根据禁用显示友情链接
        /// </summary>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public async Task<List<FriendLinkDTO>> GetFriendLinksByIsShowAsync(bool isShow)
        {

            return await Task.Run(() =>
            {
                var friendLinks = FriendLinkRepository.GetModels(a => a.id != 0);
                var friendLinkList = isShow
                    ? friendLinks.Where(a => a.isShow).ToList()
                    : friendLinks.Where(a => !a.isShow).ToList();
                return Mapper.Map<List<FriendLinkDTO>>(friendLinkList);
            });
        }

        /// <summary>
        /// 根据id删除友情链接
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntityAsync(int tId)
        {
            var friendLink = await FriendLinkRepository.GetSingleModelAsync(a => a.id == tId);
            return await FriendLinkRepository.DeleteAsync(friendLink);
        }
    }
}