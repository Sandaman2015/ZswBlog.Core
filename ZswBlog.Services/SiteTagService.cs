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
    public class SiteTagService : BaseService<SiteTagEntity, ISiteTagRepository>, ISiteTagService
    {
        public ISiteTagRepository _siteTagRepository { get; set; }
        public IMapper _mapper { get; set; }
        /// <summary>
        /// 根据禁用显示所有的站点标签
        /// </summary>
        /// <param name="isShow"></param>
        /// <returns></returns>
        public List<SiteTagDTO> GetSiteTagsByIsShow(bool isShow)
        {
            List<SiteTagEntity> siteTags = _siteTagRepository.GetModels(a => a.id != 0).ToList();
            siteTags = isShow ? siteTags.Where(a => a.isShow).ToList() : siteTags.Where(a => !a.isShow).ToList();
            return _mapper.Map<List<SiteTagDTO>>(siteTags);
        }

        /// <summary>
        /// 获取所有的站点标签
        /// </summary>
        /// <returns></returns>
        public List<SiteTagDTO> GetAllSiteTags()
        {
            List<SiteTagEntity> siteTags = _siteTagRepository.GetModels(a => a.isShow).ToList().OrderByDescending(a=>a.createDate).Take(30).ToList();
            return _mapper.Map<List<SiteTagDTO>>(siteTags);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntity(int tId)
        {
            SiteTagEntity siteTagEntity = _siteTagRepository.GetSingleModel(a => a.id == tId);
            return _siteTagRepository.Delete(siteTagEntity);
        }

        public int GetAllSiteTagsCount()
        {
            return _siteTagRepository.GetModelsCountByCondition(a => a.isShow);
        }
    }
}
