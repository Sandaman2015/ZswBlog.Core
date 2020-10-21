using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class ArticleRepository : BaseRepository<ArticleEntity>, IArticleRepository, IBaseRepository<ArticleEntity>
    {
        public List<ArticleDTO> GetArticlesByNearSave(int count)
        {
            string sql = string.Format("select a.id,a.createDate,a.operatorId,a.title,a.content,a.`like`,a.categoryId,a.visits,a.lastUpdateDate,a.isTop,a.topSort,a.readTime,a.textCount, fa.path as 'coverImage' from tab_article a left join tab_file_attachment fa on fa.id = a.coverImageId where a.isShow = 1 order by createDate desc limit {0}", count);
            IQueryable<ArticleDTO> queryable = _readDbContext.Set<ArticleDTO>().FromSqlRaw(sql, new object[0]);
            return queryable.ToList();
        }
    }
}
