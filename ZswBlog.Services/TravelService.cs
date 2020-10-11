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
    public class TravelService : BaseService<TravelEntity, ITravelRepository>, ITravelService
    {
        public ITravelRepository _travelRepository { get; set; }
        public IMapper _mapper { get; set; }

        public List<TravelDTO> GetTravels()
        {
            List<TravelEntity> travels = _travelRepository.GetModels(a => a.id != 0).ToList();
            return _mapper.Map<List<TravelDTO>>(travels);
        }

        public TravelDTO GetTravel(int tId)
        {
            TravelEntity travel = _travelRepository.GetSingleModel(t => t.id == tId);
            return _mapper.Map<TravelDTO>(travel);
        }

        public bool RemoveEntity(int tId)
        {
            TravelEntity t = new TravelEntity() { id = tId };
            return _travelRepository.Delete(t);
        }
    }
}
