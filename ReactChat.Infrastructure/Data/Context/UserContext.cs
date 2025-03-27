using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Core.Entities.Chat.Message;
using ReactChat.Core.Entities.User;
using ReactChat.Core.Enums;

namespace ReactChat.Infrastructure.Data.Context
{
    public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
    {
        public DbSet<BaseUser>? Users { get; set; }
        public DbSet<PrivateMessage>? PrivateMessages { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseUser>(entity =>
            {
                entity.ToTable("Users");
                entity.HasDiscriminator<UserRole>("UserRole")
                      .HasValue<BaseUser>(UserRole.Guest)
                      .HasValue<RegularUser>(UserRole.RegularUser)
                      .HasValue<AdminUser>(UserRole.Admin);
            });

            modelBuilder.Entity<BaseUser>()
                .Property(e => e.Accesses)
                .HasConversion<int>();
        }
    }
}
