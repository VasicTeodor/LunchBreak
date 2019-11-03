using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using LunchBreak.Helpers;
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
        public async Task<PaginationData<Restaurant>> GetRestaurants(string search, bool isAdmin = false, PaginationData<Restaurant> paginationData = null)
        {
            var pagination = paginationData ?? new PaginationData<Restaurant>() { PageSize = 5, PageNumber = 1 };
            var searchString = search ?? "";
            if (isAdmin)
            {
                return new PaginationData<Restaurant>
                {
                    Items = await _restaurants.Find(restaurant => restaurant.Name.ToLower().Contains(searchString.ToLower()))
                                        .Skip((pagination.PageNumber - 1) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync(),
                    NumberOfItems = Convert.ToInt32(await _restaurants.CountDocumentsAsync(doc => true)),
                    PageNumber = pagination.PageNumber,
                    PageSize = pagination.PageSize
                };
                //return await _restaurants.Find(restaurant => restaurant.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
            }
            else
            {
                return new PaginationData<Restaurant>{ Items = await _restaurants.Find(restaurant => restaurant.Approved == true && restaurant.Name.ToLower().Contains(searchString.ToLower()))
                                                         .Skip((pagination.PageNumber - 1) * pagination.PageSize).Limit(pagination.PageSize).ToListAsync(),
                                                            NumberOfItems = Convert.ToInt32(await _restaurants.CountDocumentsAsync(doc => doc.Approved == true)), 
                                                            PageNumber = pagination.PageNumber, PageSize = pagination.PageSize};
                //return await _restaurants.Find(restaurant => restaurant.Approved == true && restaurant.Name.ToLower().Contains(searchString.ToLower())).ToListAsync();
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

        public async Task<List<Restaurant>> GetRestaurantsAdmin()
        {
            return await _restaurants.Find(restaurant => true).ToListAsync();
        }
    }
}