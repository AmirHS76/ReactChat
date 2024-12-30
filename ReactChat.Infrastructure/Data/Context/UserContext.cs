using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Message;
using ReactChat.Core.Entities.User;

namespace ReactChat.Infrastructure.Data.Context
{
    public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
    {
        public DbSet<BaseUser>? Users { get; set; }
        public DbSet<PrivateMessage>? PrivateMessages { get; set; }
    }
}
