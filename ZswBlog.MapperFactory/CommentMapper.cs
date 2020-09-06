using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.MapperFactory
{
    public class CommentMapper
    {
        public CommentMapper(IMapper mapper)
        {
            _mapper = mapper;

        }

        private readonly IMapper _mapper;

        public async Task<List<CommentDTO>> MapperToDTOsAsync(List<Comment> comments, List<User> users)
        {
            return await Task.Run(() =>
            {
                List<CommentDTO> CommentDTOs = _mapper.Map(comments, _mapper.Map<List<CommentDTO>>(users));
                return CommentDTOs;
            });
        }

        public async Task<CommentDTO> MapperToSingleDTOAsync(Comment comment, User user)
        {
            return await Task.Run(() =>
            {
                return _mapper.Map(comment, _mapper.Map<CommentDTO>(user));
            });
        }
    }
}
