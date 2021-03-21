using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.Common;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public class TravelService : BaseService<TravelEntity, ITravelRepository>, ITravelService
    {
        public ITravelRepository TravelRepository { get; set; }
        public IMapper Mapper { get; set; }
        public ITravelFileAttachmentService _travelFileAttachmentService { get; set; }

        public async Task<PageDTO<TravelDTO>> GetTravelsByPageAsync(int pageSize, int pageIndex, bool isShow)
        {
            return await Task.Run(() =>
            {
                Expression<Func<TravelEntity, bool>> expression = a => true;
                if (isShow)
                {
                    expression = expression.And(a => a.isShow == isShow);
                }
                var travels = TravelRepository.GetModelsByPage(pageSize, pageIndex, false, a => a.createDate,
                    expression, out var total).Include(a => a.imgList).ThenInclude(a=>a.fileAttachment).ToList();
                var travelDtoList = Mapper.Map<List<TravelDTO>>(travels);
                return new PageDTO<TravelDTO>(pageIndex, pageSize, total, travelDtoList);
            });
        }

        public async Task<TravelDTO> GetTravelByIdAsync(int tId)
        {
            var travel = await TravelRepository.GetSingleModelAsync(t => t.id == tId);
            TravelDTO travelDTO =  Mapper.Map<TravelDTO>(travel);
            List<FileAttachmentEntity> fileAttachments = await _travelFileAttachmentService.GetTravelFileListByTravelId(travel.id);
            travelDTO.imgList = Mapper.Map<List<FileAttachmentDTO>>(fileAttachments);
            return travelDTO;
        }

        public async Task<bool> RemoveTravelAsync(int tId)
        {
            var t = new TravelEntity() { id = tId };
            return await TravelRepository.DeleteAsync(t);
        }
    }
}