using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class RegularUser : BaseUser
    {
        public RegularUser()
        {
            UserRole = UserRole.RegularUser;
        }
    }
}
