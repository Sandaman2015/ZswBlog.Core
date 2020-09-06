using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;

namespace ZswBlog.MapperFactory
{
    public class UserMapper
    {
        private readonly IMapper _mapper;

        public UserMapper(IMapper mapper)
        {
            this._mapper = mapper;
        }
        public async Task<UserDTO> MapperToDTOAsync(User user)
        {
            return await Task.Run(() =>
            {
                UserDTO UserDTO = _mapper.Map<UserDTO>(user);
                return UserDTO;
            });
        }

        public async Task<List<UserDTO>> MapperToDTOsAsync(List<User> users)
        {
            return await Task.Run(() =>
            {
                List<UserDTO> UserDTOs = _mapper.Map<List<UserDTO>>(users);
                return UserDTOs;
            });
        }
    }
}
