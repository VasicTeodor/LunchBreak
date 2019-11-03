using System.Collections.Generic;
using System.Threading.Tasks;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;

namespace LunchBreak.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<PaginationData<User>> GetUsers(PaginationData<User> paginationData = null);
        Task<List<User>> GetUsersAdmin();
        Task<User> GetUser(string userId);
        Task<bool> RemoveUser(string userId);
        Task<bool> UpdateUser(string userId, User user);
        Task<bool> AddUser(User newUser);
        Task<bool> CheckIfUserExists(string username);
        Task<bool> UserIsApproved(string userId);
        Task<User> GetUserByUsername(string username);
    }
}