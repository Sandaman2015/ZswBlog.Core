using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.Common.Exception;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class, new() //泛型约束必须是实体
    {
        //private readonly WritleDbContext WritleDbContext = DbContextFactory.Create();
        //public ZswBlogContext _dbContext { get; set; }

        /// <summary>
        /// 采用属性注入的方式，共享单例操作上下文，而不通过DbFactory去创建
        /// </summary>
        //public ZswBlogDbContext DbContext { get; set; }

        public WritleDbContext WritleDbContext { get; set; }

        public ReadDbContext ReadDbContext { get; set; }

        public virtual bool Add(T t)
        {
            WritleDbContext.Set<T>().Add(t);
            return WritleDbContext.SaveChanges() > 0;
        }

        public virtual bool AddList(IEnumerable<T> t)
        {
            WritleDbContext.Set<T>().AddRange(t);
            return WritleDbContext.SaveChanges() > 0;
        }

        public virtual bool Delete(T t)
        {
            //必须将给定实体附加到集的基础上下文中。也就是说，将实体以“未更改”的状态放置到上下文中，就好像从数据库读取了该实体一样。
            WritleDbContext.Set<T>().Attach(t);
            WritleDbContext.Set<T>().Remove(t);
            return WritleDbContext.SaveChanges() > 0;
        }


        public virtual bool DeleteList(IEnumerable<T> t)
        {
            WritleDbContext.Set<T>().RemoveRange(t);
            return WritleDbContext.SaveChanges() > 0;
        }

        public virtual bool Update(T t)
        {
            WritleDbContext.Set<T>().Update(t);
            return WritleDbContext.SaveChanges() > 0;
        }

        public virtual bool UpdateList(IEnumerable<T> t)
        {
            WritleDbContext.Set<IEnumerable<T>>().UpdateRange(new IEnumerable<T>[]
        {
                t
        });
            return WritleDbContext.SaveChanges() > 0;
        }

        public virtual IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return ReadDbContext.Set<T>().Where(whereLambda);
        }

        public virtual  IQueryable<T> GetModelsByPage<TType>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda, out int total)
        {
            var result = ReadDbContext.Set<T>().Where(whereLambda);
            try
            {
                total = result.Count();
                return isAsc
                    ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                    : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            }
            catch (Exception ex) {
                throw new BusinessException("查询执行错误:"+ ex.Message, 500);
            }
        }

        public virtual async Task<T> GetSingleModelAsync(Expression<Func<T, bool>> whereLambda)
        {
            return await ReadDbContext.Set<T>().Where(whereLambda).FirstOrDefaultAsync<T>();
        }

        public virtual int GetModelsCountByCondition(Expression<Func<T, bool>> whereLambda)
        {
            return whereLambda == null
                ? ReadDbContext.Set<T>().Count()
                : ReadDbContext.Set<T>().Where(whereLambda).Count();
        }

        public virtual PageEntity<T> GetModelsByPage<TType>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda)
        {
            var result = ReadDbContext.Set<T>().Where(whereLambda);
            var total = result.Count();
            var data = isAsc
                ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize)
                : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return new PageEntity<T>(pageIndex, pageSize, total, data);
        }
    }
}