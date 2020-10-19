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
        readonly static string _sqlField = "m.id,m.content,m.browser,m.location,m.userId,m.createDate,u.nickName as userName, u.portrait as userPortrait";
        public List<MessageDTO> GetMessageOnNoReply(int count)
        {
            var sql = string.Format("select {0} from tab_message m left join tab_user u on u.id = m.userId where m.targetId is null or m.targetId = 0", _sqlField);
            IQueryable<MessageDTO> messages = _readDbContext.Set<MessageDTO>().FromSqlRaw(sql, new object[0]);
            return messages.ToList();
        }
    }
}
