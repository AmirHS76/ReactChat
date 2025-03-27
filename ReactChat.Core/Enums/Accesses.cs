using System;

namespace ReactChat.Core.Enums
{
    [Flags]
    public enum Accesses
    {
        None = 0,                  // 0000 0000  0
        // Regular user accesses 
        CanSendMessage = 1 << 0,   // 0000 0001  1
        CanDeleteMessage = 1 << 1, // 0000 0010  2
        CanEditMessage = 1 << 2,   // 0000 0100  4
        // Admin user accesses     
        CanJoinGroup = 1 << 3,   // 0000 1000  8
        CanUpdateGroup = 1 << 4,   // 0001 0000  16
        CanAddUser = 1 << 5,       // 0010 0000  32
        CanRemoveUser = 1 << 6,    // 0100 0000  64
        CanUpdateUser = 1 << 7,    // 1000 0000  128
        CanDeleteGroup = 1 << 8,   // 0001 0000 0000  256
        CanCreateGroup = 1 << 9      // 0010 0000 0000 512
    }

    public class AccessesHelper
    {
        public const int FullAccess = 511;
        public const int FullUserAccess = 7;
        public const int NormalAdminAccess = 31;
    }
}
