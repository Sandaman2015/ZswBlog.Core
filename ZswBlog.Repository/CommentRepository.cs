using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class CommentRepository : BaseRepository<CommentEntity>, ICommentRepository, IBaseRepository<CommentEntity>
    {
        public List<CommentDTO> GetCommentsRecursive(int targetId, int articleId)
        {
            string sql = string.Format("WITH RECURSIVE temp AS(select m.id, m.content, m.createDate, m.articleId, m.userId, m.targetUserId, m.targetId, m.location, m.browser, u.nickName as userName, u.portrait as userPortrait, us.nickName as targetUserName, us.portrait as targetUserPortrait from tab_comment m left join tab_user u on u.id = m.userId left join tab_user us on us.id = m.targetuserId where targetId = {0} and articleId = {1} UNION ALL select m.id,m.content,m.createDate,m.articleId,m.userId,m.targetUserId,m.targetId,m.location,m.browser,t.userName,t.userPortrait,t.targetUserName, t.targetUserPortrait  from tab_comment m, temp t where m.targetId = t.id and m.targetUserId = t.userId)SELECT * FROM temp", targetId, articleId);
            IQueryable<CommentDTO> comments = _readDbContext.Set<CommentDTO>().FromSqlRaw(sql, new object[0]);
            return comments.ToList();
        }
    }
}
