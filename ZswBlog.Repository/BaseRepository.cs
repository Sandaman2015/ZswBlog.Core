using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new() //泛型约束必须是实体
    {
        //private readonly DbContext _dbContext = DbContextFactory.Create();
        //public SingleBlogContext _dbContext { get; set; }

        /// <summary>
        /// 采用属性注入的方式，共享单例操作上下文，而不通过DbFactory去创建
        /// </summary>
        public WritleDbContext _writleDbContext { get; set; }

        public ReadDbContext _readDbContext { get; set; }

        public virtual async Task<bool> AddAsync(T t)
        {
            _writleDbContext.Set<T>().AddAsync(t);
            return await _writleDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> AddListAsync(IEnumerable<T> t)
        {
            _writleDbContext.Set<T>().AddRangeAsync(t);
            return await _writleDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteAsync(T t)
        {
            //必须将给定实体附加到集的基础上下文中。也就是说，将实体以“未更改”的状态放置到上下文中，就好像从数据库读取了该实体一样。
            _writleDbContext.Set<T>().Attach(t);
            _writleDbContext.Set<T>().Remove(t);
            return await _writleDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> DeleteListAsync(IEnumerable<T> t)
        {
            _writleDbContext.Set<T>().RemoveRange(t);
            return await _writleDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> UpdateAsync(T t)
        {
            _writleDbContext.Set<T>().Update(t);
            return await _writleDbContext.SaveChangesAsync() > 0;
        }

        public virtual async Task<bool> UpdateListAsync(IEnumerable<T> t)
        {
            _writleDbContext.Set<IEnumerable<T>>().UpdateRange(new IEnumerable<T>[]
            {
                t
            });
            return await _writleDbContext.SaveChangesAsync() > 0;
        }

        public async Task<IQueryable<T>> GetModelsAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
            {
                return _readDbContext.Set<T>().Where(whereLambda);
            }); 
        }

        public IQueryable<T> GetModelsByPage<TType>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda, out int total)
        {
            var result = _readDbContext.Set<T>().Where(whereLambda);
            total = result.Count();
            return isAsc
                ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        public async Task<T> GetSingleModelAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
            {
                return _readDbContext.Set<T>().Where(whereLambda).FirstOrDefault<T>();
            });            
        }

        public async Task<int> GetModelsCountByConditionAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await Task.Run(() =>
            {
                return whereLambda == null
                    ? _readDbContext.Set<T>().Count()
                    : _readDbContext.Set<T>().Where(whereLambda).Count();
            });
        }
    }
}