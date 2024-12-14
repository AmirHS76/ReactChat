using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Login;
using ReactChat.Core.Entities.Messages;
using ReactChat.Core.Entities.User;
using ReactChat.Core.Enums;

namespace ReactChat.Infrastructure.Data.Context
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<BaseUser>? Users { get; set; }
        public DbSet<PrivateMessage>? PrivateMessages { get; set; }
    }
}
