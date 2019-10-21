using System.Collections.Generic;
using System.Threading.Tasks;
using LunchBreak.Infrastructure.Entities;

namespace LunchBreak.Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<User> GetUser(string userId);
        Task<bool> RemoveUser(string userId);
        Task<bool> UpdateUser(string userId, User user);
        Task<bool> AddUser(User newUser);
        Task<bool> CheckIfUserExists(string username);
        Task<User> GetUserByUsername(string username);
    }
}