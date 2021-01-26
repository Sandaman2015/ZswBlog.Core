﻿using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class SiteTagService : BaseService<SiteTagEntity, ISiteTagRepository>, ISiteTagService
    {
        public ISiteTagRepository SiteTagRepository { get; set; }
        public IMapper Mapper { get; set; }

        /// <summary>
        /// 根据禁用显示所有的站点标签
        /// </summary>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public async Task<List<SiteTagDTO>> GetSiteTagsByIsShowAsync(bool isShow)
        {
            return await Task.Run(() =>
            {
                var siteTags = SiteTagRepository.GetModelsAsync(a => a.id != 0).Result.ToList();
                siteTags = isShow ? siteTags.Where(a => a.isShow).ToList() : siteTags.Where(a => !a.isShow).ToList();
                return Mapper.Map<List<SiteTagDTO>>(siteTags);
            });
        }

        /// <summary>
        /// 获取所有的站点标签
        /// </summary>
        /// <returns></returns>
        public async Task<List<SiteTagDTO>> GetAllSiteTagsAsync()
        {
            return await Task.Run(() =>
            {
                var siteTags = SiteTagRepository.GetModelsAsync(a => a.isShow).Result.ToList()
                    .OrderByDescending(a => a.createDate).Take(30).ToList();
                return Mapper.Map<List<SiteTagDTO>>(siteTags);
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public async Task<bool> RemoveEntity(int tId)
        {
            var siteTagEntity = await SiteTagRepository.GetSingleModelAsync(a => a.id == tId);
            return await SiteTagRepository.DeleteAsync(siteTagEntity);
        }

        public async Task<int> GetAllSiteTagsCountAsync()
        {
            return await SiteTagRepository.GetModelsCountByConditionAsync(a => a.isShow);
        }
    }
}