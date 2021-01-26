using AutoMapper;
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
    public class TravelService : BaseService<TravelEntity, ITravelRepository>, ITravelService
    {
        public ITravelRepository TravelRepository { get; set; }
        public IMapper Mapper { get; set; }

        public async Task<PageDTO<TravelDTO>> GetTravelsByPageAsync(int pageSize, int pageIndex)
        {
            return await Task.Run(() =>
            {
                var travels = TravelRepository.GetModelsByPage(pageSize, pageIndex, false, a => a.createDate,
                    a => a.id != 0 && a.isShow, out var total).ToList();
                var travelDtoList = Mapper.Map<List<TravelDTO>>(travels);
                return new PageDTO<TravelDTO>(pageIndex, pageSize, total, travelDtoList);
            });
        }

        public async Task<TravelDTO> GetTravelAsync(int tId)
        {
            var travel = await TravelRepository.GetSingleModelAsync(t => t.id == tId);
            return Mapper.Map<TravelDTO>(travel);
        }

        public async Task<bool> RemoveEntityAsync(int tId)
        {
            var t = new TravelEntity() {id = tId};
            return await TravelRepository.DeleteAsync(t);
        }
    }
}