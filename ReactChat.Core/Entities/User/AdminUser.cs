using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class AdminUser : BaseUser
    {
#pragma warning disable IDE0051 // Remove unused private members
        AdminAccesses AdminAccesses { get; set; }
        public AdminUser()
        {
            Role = UserRole.Admin;
        }
    }
}
