using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Login;
using ReactChat.Core.Entities.Messages;
using ReactChat.Core.Entities.User;
using ReactChat.Core.Enums;

namespace ReactChat.Infrastructure.Data
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options) { }
        public DbSet<BaseUser> Users { get; set; }
        public DbSet<PrivateMessage> PrivateMessages { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BaseUser>()
                .HasDiscriminator<UserRole>("Role")
                .HasValue<RegularUser>(UserRole.RegularUser)
                .HasValue<AdminUser>(UserRole.Admin)
                .HasValue<BaseUser>(UserRole.Guest);
        }
    }
}
