using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleRepository : BaseRepository<ArticleEntity>, IArticleRepository, IBaseRepository<ArticleEntity>
    {
        public async override Task<PageEntity<ArticleEntity>> GetModelsByPageAsync<TType>(int pageSize, int pageIndex, bool isAsc, Expression<Func<ArticleEntity, TType>> orderByLambda, Expression<Func<ArticleEntity, bool>> whereLambda)
        {
            var result = DbContext.Set<ArticleEntity>().Where(whereLambda);
            var total = await result.CountAsync();
            var data = isAsc
                ? result.OrderBy(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).Include(a => a.category).Include(a => a.articleTags).ThenInclude(a => a.tag)
                : result.OrderByDescending(orderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize).Include(a => a.category).Include(a => a.articleTags).ThenInclude(a => a.tag);
            return new PageEntity<ArticleEntity>(pageIndex, pageSize, total, data);
        }
    }
}
