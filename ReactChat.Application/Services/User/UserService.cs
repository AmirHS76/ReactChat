using MediatR;
using ReactChat.Application.Features.Queries;
using ReactChat.Application.Interfaces.Cache;
using ReactChat.Application.Interfaces.User;
using ReactChat.Core.Entities.User;
using ReactChat.Core.Enums;
using ReactChat.Infrastructure.Data.UnitOfWork;

namespace ReactChat.Application.Services.User
{
    public class UserService(IUnitOfWork unitOfWork, ICacheService cacheService, IMediator mediator) : IUserService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ICacheService _cacheService = cacheService;
        private readonly TimeSpan _cacheExpiration = TimeSpan.FromMinutes(5);
        private readonly IMediator _mediator = mediator;
        public async Task<BaseUser?> GetUserByUsernameAsync(string? username)
        {
            if (username == null) return null;
            BaseUser? user = await _unitOfWork.UserRepository.GetUserByUsernameAsync(username);
            return user;
        }

        public async Task<bool> UpdateUserAsync(int id, string username, string email)
        {
            BaseUser? user;
            if (id == 0)
                user = await GetUserByUsernameAsync(username);
            else
                user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return false;
            user.Username = username;
            user.Email = email;
            await _unitOfWork.UserRepository.UpdateAsync(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<BaseUser>?> GetAllUsersAsync()
        {
            var cacheKey = "AllUsers";
            //var cachesUsers = await _cacheService.GetAsync<IEnumerable<BaseUser>>(cacheKey);
            //if (cachesUsers != null)
            //    return cachesUsers;

            IEnumerable<BaseUser> allUsers = await _mediator.Send(new GetAllUsersQuery());
            if (allUsers != null)
                await _cacheService.SetAsync(cacheKey, allUsers, _cacheExpiration);

            return allUsers;
        }

        public async Task<bool> AddNewUserAsync(string username, string password, string email, string role)
        {
            BaseUser? baseUser;
            baseUser = await GetUserByUsernameAsync(username);
            if (baseUser != null)
                return false;
            baseUser = CreateUser(username, password, email, role);
            await _unitOfWork.UserRepository.AddAsync(baseUser);
            return true;
        }

        private static BaseUser? CreateUser(string username, string password, string email, string role)
        {
            if (!Enum.TryParse(role, out UserRole userRole))
                return null;

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

            return userRole switch
            {
                UserRole.Admin => new AdminUser
                {
                    Username = username,
                    Password = hashedPassword,
                    Email = email,
                    UserRole = userRole,
                    Accesses = Accesses.CanCreateGroup | Accesses.CanUpdateGroup | Accesses.CanDeleteGroup | Accesses.CanRemoveUser | Accesses.CanUpdateUser
                },
                UserRole.RegularUser => new RegularUser
                {
                    Username = username,
                    Password = hashedPassword,
                    Email = email,
                    UserRole = userRole,
                    Accesses = Accesses.CanSendMessage | Accesses.CanDeleteMessage | Accesses.CanEditMessage
                },
                _ => new BaseUser
                {
                    Username = username,
                    Password = hashedPassword,
                    Email = email,
                    UserRole = userRole,
                    Accesses = Accesses.None
                }
            };
        }

        public async Task<bool> DeleteUserByID(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
            if (user == null)
                return false;
            await _unitOfWork.UserRepository.DeleteAsync(id);
            return true;
        }
    }
}
