using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.MapperFactory
{
    public class FriendLinkMapper
    {
        private readonly IMapper _mapper;

        public FriendLinkMapper(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public async Task<List<FriendLinkDTO>> MapperToDTOsAsync(List<FriendLink> friendLinks)
        {
            return await Task.Run(() =>
            {
                return _mapper.Map<List<FriendLinkDTO>>(friendLinks);
            });
        }
    }
}
