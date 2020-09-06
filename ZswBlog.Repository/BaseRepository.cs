using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public abstract class BasicRepository<T> : IBaseRepository<T> where T : class, new()//泛型约束必须是实体
    {
        //private readonly DbContext _dbContext = DbContextFactory.Create();
        public SingleBlogContext _dbContext { get; set; }//采用属性注入的方式，共享单例操作上下文，而不通过DbFactory去创建

        //protected BasicRepository(SingleBlogContext dbContext)
        //{
        //    _dbContext = dbContext;
        //}

        public bool Add(T t)
        {
            this._dbContext.Set<T>().Add(t);
            return this._dbContext.SaveChanges() > 0;
        }

        public bool Add(IEnumerable<T> t)
        {
            this._dbContext.Set<T>().AddRange(t);
            return this._dbContext.SaveChanges() > 0;
        }

        public bool Delete(T t)
        {
            this._dbContext.Set<T>().Attach(t);//必须将给定实体附加到集的基础上下文中。也就是说，将实体以“未更改”的状态放置到上下文中，就好像从数据库读取了该实体一样。
            this._dbContext.Set<T>().Remove(t);
            //this._dbContext.Entry<T>(t).State = EntityState.Deleted;            
            return this._dbContext.SaveChanges() > 0;
        }

        public bool Delete(IEnumerable<T> t)
        {
            this._dbContext.Set<T>().RemoveRange(t);
            return this._dbContext.SaveChanges() > 0;
        }

        public bool Update(T t)
        {
            this._dbContext.Set<T>().Update(t);
            return this._dbContext.SaveChanges() > 0;
        }

        public bool Update(IEnumerable<T> t)
        {
            this._dbContext.Set<IEnumerable<T>>().UpdateRange(new IEnumerable<T>[]
            {
                t
            });
            return this._dbContext.SaveChanges() > 0;
        }

        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return this._dbContext.Set<T>().Where(whereLambda);
        }

        public IQueryable<T> GetModelsByPage<TType>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, TType>> orderByLambda, Expression<Func<T, bool>> whereLambda, out int total)
        {
            IQueryable<T> result = this._dbContext.Set<T>().Where(whereLambda);
            total = result.Count<T>();
            if (isAsc)
            {
                return result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);//Take()取指定序列的元素
            }
            return result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

        [Obsolete]
        public IQueryable<T> GetModelsBySql(string sql)
        {
            return this._dbContext.Query<T>().FromSql(sql, new object[0]);
        }

        //public DbRawSqlQuery<T> GetModelsBySqlPage<TType>(string sql, int pageSize, int pageIndex, out int total)
        //{
        //    DbRawSqlQuery<T> result = this._dbContext.Database.SqlQuery<T>(sql, new object[0]);
        //    total = result.Count<T>();
        //    return (DbRawSqlQuery<T>)result.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        //}

        public T GetSingleModel(Expression<Func<T, bool>> whereLambda)
        {
            return this._dbContext.Set<T>().Where(whereLambda).FirstOrDefault<T>();
        }
    }



}
