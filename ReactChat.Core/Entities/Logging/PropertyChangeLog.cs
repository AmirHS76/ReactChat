using System;

namespace ReactChat.Core.Entities.Logging
{
    public class PropertyChangeLog
    {
        public int Id { get; set; }
        public string? EntityName { get; set; }
        public string? PropertyName { get; set; }
        public string? PrimaryKeyValue { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;
        public string? ChangedBy { get; set; }
    }
}
