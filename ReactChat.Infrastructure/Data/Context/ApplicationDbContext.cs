using Microsoft.EntityFrameworkCore;
using ReactChat.Core.Entities.Logging;

namespace ReactChat.Infrastructure.Data.Context
{
    public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<PropertyChangeLog> PropertyChangeLogs { get; set; }

        public override int SaveChanges()
        {
            var logs = CollectChangeLogs();
            var result = base.SaveChanges();

            if (logs.Count != 0)
            {
                PropertyChangeLogs.AddRange(logs);
                base.SaveChanges();
            }

            return result;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var logs = CollectChangeLogs();
            var result = await base.SaveChangesAsync(cancellationToken);

            if (logs.Count != 0)
            {
                try
                {
                    PropertyChangeLogs.AddRange(logs);
                    await base.SaveChangesAsync(cancellationToken);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error happened when saving changes : " + ex.Message);
                }
            }

            return result;
        }

        private List<PropertyChangeLog> CollectChangeLogs()
        {
            ChangeTracker.DetectChanges();

            var logs = new List<PropertyChangeLog>();
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries().Where(e => e.State == EntityState.Modified))
            {
                var entityName = entry.Entity.GetType().Name;
                var primaryKey = entry.Properties.FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString();

                foreach (var prop in entry.Properties)
                {
                    if (prop.IsModified)
                    {
                        var oldValue = prop.OriginalValue?.ToString();
                        var newValue = prop.CurrentValue?.ToString();

                        if (oldValue != newValue)
                        {
                            logs.Add(new PropertyChangeLog
                            {
                                EntityName = entityName,
                                PrimaryKeyValue = primaryKey,
                                PropertyName = prop.Metadata.Name,
                                OldValue = oldValue,
                                NewValue = newValue,
                                ChangedAt = now,
                                ChangedBy = GetCurrentUserId()
                            });
                        }
                    }
                }
            }

            return logs;
        }

        private string GetCurrentUserId()
        {
            return "system";
        }
    }
}
