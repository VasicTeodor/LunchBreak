using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using LunchBreak.Infrastructure.DatabaseSettings;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace LunchBreak.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _userRepository;
        private readonly string _collectionName = "Users";

        public UserRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _userRepository = database.GetCollection<User>(_collectionName);
        }
        public async Task<List<User>> GetUsers() => await _userRepository.Find(user => true).ToListAsync();

        public async Task<User> GetUser(string userId) => await _userRepository.Find(user => user.Id.Equals(userId)).FirstOrDefaultAsync();

        public async Task<bool> RemoveUser(string userId) => (await _userRepository.DeleteOneAsync(user => user.Id.Equals(userId))).DeletedCount > 0;

        public async Task<bool> UpdateUser(string userId, User user)
        {
            user.Id = userId;

            try
            {
                var result = await _userRepository.ReplaceOneAsync(user => user.Id.Equals(userId), user);
                return result.ModifiedCount > 0;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error while saving changes: {e.Message}", "ERROR");
                return false;
            }
        }

        public async Task<bool> AddUser(User newUser)
        {
            try
            {
                newUser.Id = Guid.NewGuid().ToString();
                await _userRepository.InsertOneAsync(newUser);
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error while adding new user: {e.Message}", "ERROR");
                return false;
            }
        }

        public async Task<bool> CheckIfUserExists(string username) => await _userRepository
            .Find(user => user.Username.Equals(username)).AnyAsync();

        public async Task<User> GetUserByUsername(string username) => await _userRepository
            .Find(user => user.Username.Equals(username)).FirstOrDefaultAsync();
    }
}