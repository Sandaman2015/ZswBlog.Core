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
    public class TimeLineService : BaseService<TimeLineEntity, ITimeLineRepository>, ITimeLineService
    {
        private readonly ITimeLineRepository _timeLineRepository;
        private readonly IMapper _mapper;

        public TimeLineService(ITimeLineRepository timeLineRepository, IMapper mapper)
        {
            _timeLineRepository = timeLineRepository;
            _mapper = mapper;
        }
        /// <summary>
        /// 删除时间线
        /// </summary>
        /// <param name="tId"></param>
        /// <returns></returns>
        public bool RemoveEntity(int tId)
        {
            TimeLineEntity timeLine = _timeLineRepository.GetSingleModel(a => a.id == tId);
            return _timeLineRepository.Delete(timeLine);
        }

        /// <summary>
        /// 获取所有时间线列表
        /// </summary>
        /// <returns></returns>
        public List<TimeLineDTO> GetTimeLineList()
        {
            List<TimeLineEntity> timeLines = _timeLineRepository.GetModels(a => a.id != 0).OrderByDescending(a=>a.createDate).ToList();
            return _mapper.Map<List<TimeLineDTO>>(timeLines);
        }
    }
}
