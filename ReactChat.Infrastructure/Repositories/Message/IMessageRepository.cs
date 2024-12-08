﻿using ReactChat.Core.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactChat.Infrastructure.Repositories.Message
{
    public interface IMessageRepository : IGenericRepository<PrivateMessage>
    {
    }
}
