using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Login;
using ReactChat.Core.Entities.Messages;

namespace ReactChat.Infrastructure.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<BaseUser> Users { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
    }
}
