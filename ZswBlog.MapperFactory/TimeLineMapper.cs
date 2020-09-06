using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.MapperFactory
{
    public class TimeLineMapper
    {
        private readonly IMapper _mapper;

        public TimeLineMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<TimeLineDTO>> MapperToDTOsAsync(List<Timeline> timeLines)
        {
            return await Task.Run(() =>
            {
                List<TimeLineDTO> timeLineDTOs = _mapper.Map<List<TimeLineDTO>>(timeLines);
                return timeLineDTOs;
            });
        }
    }
}
