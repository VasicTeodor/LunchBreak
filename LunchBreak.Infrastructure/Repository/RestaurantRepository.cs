using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LunchBreak.Infrastructure.DatabaseSettings;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using MongoDB.Driver;

namespace LunchBreak.Infrastructure.Repository
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly IMongoCollection<Restaurant> _restaurants;
        private readonly string _collectionName = "Restaurants";
        public RestaurantRepository(IDatabaseSettings databaseSettings)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);

            _restaurants = database.GetCollection<Restaurant>(_collectionName);
        }
        public async Task<List<Restaurant>> GetRestaurants(bool isAdmin = false)
        {
            if (isAdmin)
            {
                return await _restaurants.Find(restaurant => true).ToListAsync();
            }
            else
            {
                return await _restaurants.Find(restaurant => restaurant.Approved == true).ToListAsync();
            }
        }
        public async Task<Restaurant> GetRestaurant(string restaurantId) => await _restaurants.Find(restaurant => restaurant.Id.Equals(restaurantId)).FirstOrDefaultAsync();

        public async Task<bool> AddRestaurant(Restaurant newRestaurant)
        {
            try
            {
                newRestaurant.Id = Guid.NewGuid().ToString();
                await _restaurants.InsertOneAsync(newRestaurant);
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error while adding new restaurant: {e.Message}", "ERROR");
                return false;
            }
        }

        public async Task<bool> RemoveRestaurant(string restaurantId) => (await _restaurants.DeleteOneAsync(restaurant => restaurant.Id.Equals(restaurantId))).DeletedCount > 0;

        public async Task<bool> UpdateRestaurant(string restaurantId, Restaurant restaurant)
        {
            restaurant.Id = restaurantId;

            try
            {
                var result = await _restaurants.ReplaceOneAsync(res => res.Id.Equals(restaurantId), restaurant);
                return result.ModifiedCount > 0;
            }
            catch (Exception e)
            {
                Trace.WriteLine($"Error while saving changes: {e.Message}", "ERROR");
                return false;
            }
        }
    }
}