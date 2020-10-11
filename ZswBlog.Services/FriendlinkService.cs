using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class FriendLinkService : BaseService<FriendLinkEntity, IFriendLinkRepository>, IFriendLinkService
    {
        public IFriendLinkRepository _friendLinkRepository { get; set; }
        public IMapper _mapper { get; set; }
        /// <summary>
        /// 获取所有友情链接
        /// </summary>
        /// <returns></returns>
        public List<FriendLinkDTO> GetAllFriendLinks()
        {
            List<FriendLinkEntity> friendLinks = _friendLinkRepository.GetModels(a => a.id != 0).ToList();
            return _mapper.Map<List<FriendLinkDTO>>(friendLinks);
        }

        /// <summary>
        /// 根据禁用显示友情链接
        /// </summary>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public List<FriendLinkDTO> GetFriendLinksByIsShow(bool isShow)
        {
            List<FriendLinkEntity> friendLinks = _friendLinkRepository.GetModels(a => a.id != 0).ToList();
            friendLinks = isShow ? friendLinks.Where(a => a.isShow).ToList() : friendLinks.Where(a => !a.isShow).ToList();
            return _mapper.Map<List<FriendLinkDTO>>(friendLinks);
        }

        /// <summary>
        /// 根据id删除友情链接
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntityAsync(int tId)
        {
            FriendLinkEntity friendLink = _friendLinkRepository.GetSingleModel(a => a.id == tId);
            return _friendLinkRepository.Delete(friendLink);
        }
    }
}
