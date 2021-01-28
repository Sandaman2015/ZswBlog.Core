using Autofac.Extras.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ZswBlog.Common.AopConfig;
using ZswBlog.IRepository;
using ZswBlog.IServices;

namespace ZswBlog.Services
{
    public abstract class BaseService<T, D> : IBaseService<T> where T : class, new() where D : IBaseRepository<T>
    {
        public IBaseRepository<T> Repository { get; set; }

        public virtual async Task<bool> AddEntityAsync(T t)
        {
            return await Repository.AddAsync(t);
        }

        public virtual async Task<int> GetEntitiesCountAsync()
        {
            return await Repository.GetModelsCountByConditionAsync(null);
        }

        public virtual async Task<bool> UpdateEntityAsync(T t)
        {
            return await Repository.UpdateAsync(t);
        }
    }
}
