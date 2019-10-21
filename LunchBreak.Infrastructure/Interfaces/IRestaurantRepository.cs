using System.Collections.Generic;
using System.Threading.Tasks;
using LunchBreak.Infrastructure.Entities;

namespace LunchBreak.Infrastructure.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<List<Restaurant>> GetRestaurants(bool isAdmin = false);
        Task<Restaurant> GetRestaurant(string restaurantId);
        Task<bool> AddRestaurant(Restaurant newRestaurant);
        Task<bool> RemoveRestaurant(string restaurantId);
        Task<bool> UpdateRestaurant(string restaurantId, Restaurant restaurant);
    }
}