using ReactChat.Core.Enums;

namespace ReactChat.Core.Entities.User
{
    public class RegularUser : BaseUser
    {
        RegularAccesses RegularAccesses { get; set; }
        public RegularUser()
        {
            Role = base.Role;
        }
    }
}
