using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.Query;
using ZswBlog.Util;

namespace ZswBlog.Services
{
    public class UserService : BaseService<UserEntity, IUserRepository>, IUserService
    {

        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IQQUserInfoService _userQQInfoService;
        public UserService(IUserRepository repository, IQQUserInfoService userQQInfoService, IMapper mapper)
        {
            _userRepository = repository;
            _userQQInfoService = userQQInfoService;
            _mapper = mapper;
        }

        public List<UserDTO> GetAllUsers()
        {
            List<UserEntity> users = _userRepository.GetModels(a => a.id != 0).ToList();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public UserDTO GetUserByOpenId(string openId)
        {
            QQUserInfoEntity infoEntity = _userQQInfoService.GetQQUserInfoByOpenId(openId);
            if (infoEntity != null)
            {
                UserEntity user = _repository.GetSingleModel(a => a.id == infoEntity.userId);
                return _mapper.Map<UserDTO>(user);
            }
            else {
                return null;
            }
        }

        public UserDTO GetUserById(int id)
        {
            UserEntity user = _repository.GetSingleModel(a => a.id == id);
            return _mapper.Map<UserDTO>(user);
        }

        public bool AddEntity(UserSaveQuery t)
        {
            try
            {
                bool isOk = false;
                if (!string.IsNullOrEmpty(t.accessToken) && !string.IsNullOrEmpty(t.openId))
                {

                    UserEntity user = _mapper.Map<UserEntity>(t);
                    QQUserInfoEntity infoEntity = _mapper.Map<QQUserInfoEntity>(t);
                    if (user != null)
                    {
                        //该用户已经登陆过了，只需要更新登录信息就行了
                        user.lastLoginDate = DateTime.Now;
                        user.loginCount += 1;
                        user.nickName = t.nickName;
                        user.portrait = t.portrait;
                        infoEntity.userId = user.id;
                        isOk = _userRepository.Update(user) && _userQQInfoService.UpdateEntityAsync(infoEntity);                        
                    }
                    else
                    {
                        string defaultPwd = MD5Helper.GetMD5String("123456");//默认使用MD5加密密码         
                        user = new UserEntity
                        {
                            //该用户未登录需要填入信息
                            lastLoginDate = DateTime.Now,
                            loginCount = 1,
                            loginTime = DateTime.Now,
                            createDate = DateTime.Now,
                            password = defaultPwd
                        };
                        infoEntity.accessToken = t.accessToken;
                        infoEntity.openId = t.openId;
                        infoEntity.nickName = t.nickName;
                        infoEntity.figureurl_qq_1 = t.portrait;
                        isOk = _userRepository.Add(user) && _userQQInfoService.UpdateEntityAsync(infoEntity);
                    }
                }
                return isOk;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<UserDTO> GetUsersNearVisit(int count)
        {
            List<UserEntity> users = _repository.GetModels(a => a.id != 0).OrderByDescending(a => a.createDate).Take(count).ToList();
            return _mapper.Map<List<UserDTO>>(users);
        }

        public bool RemoveEntity(int tId)
        {
            return _userRepository.Delete(new UserEntity { id = tId });
        }
    }
}
