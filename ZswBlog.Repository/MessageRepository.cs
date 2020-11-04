using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using ZswBlog.DTO;
using ZswBlog.Entity;
using ZswBlog.IRepository;

namespace ZswBlog.Repository
{
    public class MessageRepository : BaseRepository<MessageEntity>, IMessageRepository, IBaseRepository<MessageEntity>
    {
        readonly static string _baseSqlField = "id,content,createDate,userId,targetUserId,targetId,location,browser";
        readonly static string _sqlField = "m.id,m.content,m.browser,m.location,m.userId,m.createDate,u.nickName as userName, u.portrait as userPortrait, null as targetUserPortrait, null as targetUserName";
        public List<MessageDTO> GetMessageOnNoReply(int count)
        {
            var sql = string.Format("select {0} from tab_message m left join tab_user u on u.id = m.userId where m.targetId is null or m.targetId = 0", _sqlField);
            IQueryable<MessageDTO> messages = _readDbContext.Set<MessageDTO>().FromSqlRaw(sql, new object[0]);
            return messages.ToList();
        }

        public List<MessageDTO> GetMessagesRecursive(int targetId)
        {
            var sql = string.Format("WITH RECURSIVE temp AS(select m.id, m.content, m.createDate, m.userId, m.targetUserId, m.targetId, m.location, m.browser, u.nickName as userName, u.portrait as userPortrait, us.nickName as targetUserName, us.portrait as targetUserPortrait from tab_message m left join tab_user u on u.id = m.userId left join tab_user us on us.id = m.targetuserId where targetId = {0} UNION ALL select  m.id, m.content, m.createDate, m.userId,m.targetUserId,m.targetId,m.location,m.browser,t.userName,t.userPortrait,t.targetUserName, t.targetUserPortrait from tab_message m, temp t where m.targetId = t.id and m.targetUserId = t.userId)SELECT * FROM temp", targetId);
            IQueryable<MessageDTO> messages = _readDbContext.Set<MessageDTO>().FromSqlRaw(sql, new object[0]);
            return messages.ToList();
        }
    }
}
