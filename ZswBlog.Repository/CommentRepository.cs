using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class CommentRepository : BaseRepository<CommentEntity>, ICommentRepository, IBaseRepository<CommentEntity>
    {
        public async Task<List<CommentDTO>> GetCommentsRecursiveAsync(int targetId, int articleId)
        {
            return await Task.Run(() =>
            {
                string sql = string.Format(
                    "WITH RECURSIVE temp AS(select m.id, m.content, m.createDate, m.articleId, m.userId, m.targetUserId, m.targetId, m.location, m.browser from tab_comment m where m.targetId = {0} and m.articleId = {1} UNION ALL select m.id,m.content,m.createDate,m.articleId,m.userId,m.targetUserId,m.targetId,m.location,m.browser from tab_comment m, temp t where m.targetId = t.id)SELECT t.*,u.nickName as userName,u.portrait as userPortrait, us.nickName as targetUserName, us.portrait as targetUserPortrait FROM temp t left join tab_user u on u.id = t.userId left join tab_user us on us.id = t.targetuserId ",
                    targetId, articleId);
                IQueryable<CommentDTO> comments = ReadDbContext.Set<CommentDTO>().FromSqlRaw(sql, new object[0]);
                return comments.ToList();
            });
        }
    }
}