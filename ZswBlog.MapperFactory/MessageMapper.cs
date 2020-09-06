using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.MapperFactory
{
    public class MessageMapper
    {
        public MessageMapper(IMapper mapper)
        {
            _mapper = mapper;

        }

        private readonly IMapper _mapper;

        public async Task<List<MessageDTO>> MapperToDTOsAsync(List<Message> messages, List<User> users)
        {
            return await Task.Run(() =>
            {
                List<MessageDTO> MessageDTOs = _mapper.Map(messages, _mapper.Map<List<MessageDTO>>(users));
                return MessageDTOs;
            });
        }

        public async Task<MessageDTO> MapperToSingleDTOAsync(Message message, User user)
        {
            return await Task.Run(() =>
            {
                return _mapper.Map(message, _mapper.Map<MessageDTO>(user));
            });
        }
    }
}
