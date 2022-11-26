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
                var siteTags = SiteTagRepository.GetModels(a => a.id != 0);
                var siteTagList = isShow ? siteTags.Where(a => a.isShow).ToList() : siteTags.Where(a => !a.isShow).ToList();
                return Mapper.Map<List<SiteTagDTO>>(siteTagList);
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
                var siteTags = SiteTagRepository.GetModels(a => a.isShow);
                return Mapper.Map<List<SiteTagDTO>>(siteTags.OrderByDescending(a => a.createDate).Take(30).ToList());
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
            return SiteTagRepository.Delete(siteTagEntity);
        }

        public int GetAllSiteTagsCount()
        {
            return SiteTagRepository.GetModelsCountByCondition(a => a.isShow);
        }

        public bool RemoveSiteTagById(SiteTagEntity tagEntity)
        {
            return SiteTagRepository.Delete(tagEntity);
        }
    }
}