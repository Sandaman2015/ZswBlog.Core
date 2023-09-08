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
        /// 获取友情链接详情
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<FriendLinkDTO> GetFriendLinkByIdAsync(int tId)
        {
            return await Task.Run(() =>
            {
                var friendLink = FriendLinkRepository.GetSingleModel(a => a.id == tId);
                return Mapper.Map<FriendLinkDTO>(friendLink);
            });
        }

        public async Task<PageDTO<FriendLinkDTO>> GetFriendLinkListByPageAsync(int limit, int pageIndex)
        {
            return await Task.Run(() =>
            {
                var friendLinks = FriendLinkRepository.GetModelsByPage(limit, pageIndex, true, a => a.createDate,
                    a =>a.id!=0, out var total);
                var list = Mapper.Map<List<FriendLinkDTO>>(friendLinks.ToList());
                return new PageDTO<FriendLinkDTO>(pageIndex, limit, total, list);
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
        public bool RemoveFriendLinkById(int tId)
        {
            var friendLink = new FriendLinkEntity { id = tId };
            return FriendLinkRepository.Delete(friendLink);
        }

        /// <summary>
        /// 更新友情连接
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool UpdateFriendLink(FriendLinkEntity entity)
        {
            return FriendLinkRepository.Update(entity);
        }
    }
}