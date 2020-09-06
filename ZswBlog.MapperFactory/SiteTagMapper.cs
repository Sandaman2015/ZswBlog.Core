using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.MapperFactory
{
    public class SiteTagMapper
    {
        private readonly IMapper _mapper;

        public SiteTagMapper(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<List<SiteTagDTO>> MapperToDTOsAsync(List<SiteTag> siteTags)
        {
            return await Task.Run(() =>
            {
                List<SiteTagDTO> siteTagDTOs = _mapper.Map<List<SiteTagDTO>>(siteTags);
                return siteTagDTOs;
            });
        }
    }
}
