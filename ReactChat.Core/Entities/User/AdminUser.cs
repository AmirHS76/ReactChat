using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class AdminUser : BaseUser
    {
        public AdminUser()
        {
            UserRole = UserRole.Admin;
            Accesses = (Accesses)7;
        }
    }
}
