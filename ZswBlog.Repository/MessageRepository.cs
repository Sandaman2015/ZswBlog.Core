using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.Entity.DbContext;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository, IBaseRepository<MessageEntity>
    {
        readonly static string _sqlField = "m.id,m.content,m.browser,m.location,m.userId,m.createDate,u.nickName as userName, u.portrait as userPortrait, null as targetUserPortrait, null as targetUserName, m.ip,m.isShow";
        public List<MessageDTO> GetMessageOnNoReply(int count)
        {
            var sql = string.Format("select {0} from tab_message m left join tab_user u on u.id = m.userId where m.targetId is null or m.targetId = 0 AND m.isShow = 1", _sqlField);
            IQueryable<MessageDTO> messages = DbContext.Set<MessageDTO>().FromSqlRaw(sql, Array.Empty<Object>());
            return messages.ToList();
        }
        public List<MessageDTO> GetMessageOnNearSave(int count)
        {
            var sql = string.Format("select {0} from tab_message m left join tab_user u on u.id = m.userId WHERE m.isShow = 1 order by createDate desc limit {1}", _sqlField, count);
            IQueryable<MessageDTO> messages = DbContext.Set<MessageDTO>().FromSqlRaw(sql, Array.Empty<Object>());
            return messages.ToList();
        }

        public List<MessageDTO> GetMessagesRecursive(int targetId)
        {
            var sql = string.Format("WITH RECURSIVE temp AS(SELECT m.id, m.content, m.createDate, m.userId,m.ip, m.targetUserId, m.targetId, m.location, m.browser,m.isShow FROM tab_message m WHERE targetId = {0} UNION ALL SELECT  m.id, m.content, m.createDate, m.userId,m.ip, m.targetUserId, m.targetId, m.location, m.browser,m.isShow FROM tab_message m, temp t WHERE m.targetId = t.id) SELECT t.*,us.nickName AS targetUserName, us.portrait AS targetUserPortrait, u.nickName as userName, u.portrait AS userPortrait  FROM temp t LEFT JOIN tab_user u ON u.id = t.userId LEFT JOIN tab_user us ON us.id = t.targetUserId WHERE t.isShow = 1", targetId);
            IQueryable<MessageDTO> messages = DbContext.Set<MessageDTO>().FromSqlRaw(sql);
            return messages.ToList();
        }
    }
}
