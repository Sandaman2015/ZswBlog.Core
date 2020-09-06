using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.Entity;
using ZswBlog.IRepository;
using ZswBlog.IServices;
using ZswBlog.Util;

namespace Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        private IUserRepository _repository;

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await Task.Run(() =>
            {
                List<User> users = _repository.GetModels(a => a.UserId != null).ToList();
                return users;
            });
        }

        public async Task<User> GetUserByOpenIdAsync(string openId)
        {
            return await Task.Run(() =>
            {
                User user = _repository.GetSingleModel(a => a.UserOpenId == openId);
                return user;
            });
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await Task.Run(() =>
            {
                User user = _repository.GetSingleModel(a => a.UserId == id);
                return user;
            });
        }

        public async Task<bool> AddEntityAsync(User t)
        {
            return await Task.Run(() =>
            {
                try
                {
                    bool isOk = false;
                    if (!string.IsNullOrEmpty(t.UserAccessToken) && !string.IsNullOrEmpty(t.UserOpenId))
                    {

                        User user = _repository.GetSingleModel(a => a.UserOpenId == t.UserOpenId);
                        if (user != null)
                        {
                            //该用户已经登陆过了，只需要更新登录信息就行了
                            user.UserAccessToken = t.UserAccessToken;
                            user.UserLastLoginTime = user.UserLoginTime;
                            user.UserLoginCount += 1;
                            user.UserName = t.UserName;
                            user.UserPortrait = t.UserPortrait;
                            user.UserLoginTime = DateTime.Now;
                            isOk = _repository.Update(user);
                        }
                        else
                        {
                            string defaultPwd = MD5Helper.GetMD5String("123456");//默认使用MD5加密密码         
                            user = new User
                            {
                                //该用户未登录需要填入信息
                                UserAccessToken = t.UserAccessToken,
                                UserLastLoginTime = DateTime.Now,
                                UserLoginCount = 1,
                                UserName = t.UserName,
                                UserPortrait = t.UserPortrait,
                                UserLoginTime = DateTime.Now,
                                UserCreateTime = DateTime.Now,
                                UserOpenId = t.UserOpenId,
                                UserPassword = defaultPwd,
                                ITCode = t.UserName
                            };
                            isOk = _repository.Add(user);
                        }
                    }
                    return isOk;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });
        }
        public async Task<bool> AlterEntityAsync(User t)
        {
            return await Task.Run(() =>
            {
                return _repository.Update(t);
            });
        }

        public async Task<List<User>> GetUsersNearVisit(int count)
        {
            return await Task.Run(() =>
            {
                List<User> users = _repository.GetModels(a => a.UserId != Guid.Empty && a.Discriminator == "User").OrderByDescending(a => a.UserLoginTime).Take(count).ToList();
                return users;
            });
        }

        public Task<bool> RemoveEntityAsync(int tId)
        {
            throw new NotImplementedException();
        }
    }
}
