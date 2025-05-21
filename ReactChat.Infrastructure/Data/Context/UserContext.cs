using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Chat.Group;
using ReactChat.Core.Entities.Chat.Message;
using ReactChat.Core.Entities.User;
using ReactChat.Core.Enums;

namespace ReactChat.Infrastructure.Data.Context
{
    public class UserContext : ApplicationDbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
        }

        public DbSet<BaseUser>? Users { get; set; }
        public DbSet<PrivateMessage>? PrivateMessages { get; set; }
        public DbSet<ChatGroup> ChatGroups { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserSession> UserSessions { get; set; }

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

            modelBuilder.Entity<UserGroup>()
                .HasOne(ug => ug.Group)
                .WithMany()
                .HasForeignKey(ug => ug.GroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
