using ReactChat.Core.Entities.Messages;
using ReactChat.Infrastructure.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public class MessageRepository : GenericRepository<PrivateMessage>, IMessageRepository
    {
        private readonly UserContext _context;

        public MessageRepository(UserContext context) : base(context)
        {
            _context = context;
        }
    }
}
