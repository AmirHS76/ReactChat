using System;

namespace ReactChat.Core.Enums
{
    [Flags]
    public enum Accesses
    {
        None = 0,
        // Regular user accesses
        CanSendMessage = 1 << 0,   // 1
        CanDeleteMessage = 1 << 1, // 2
        CanEditMessage = 1 << 2,   // 4
        // Admin user accesses
        CanCreateGroup = 1 << 3,   // 8
        CanUpdateGroup = 1 << 4,   // 16
        CanDeleteGroup = 1 << 5,   // 32
        CanRemoveUser = 1 << 6,    // 64
        CanUpdateUser = 1 << 7     // 128
    }
}
