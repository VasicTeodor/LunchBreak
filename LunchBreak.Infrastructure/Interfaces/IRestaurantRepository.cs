using System.Collections.Generic;
using System.Threading.Tasks;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;

namespace LunchBreak.Infrastructure.Interfaces
{
    public interface IRestaurantRepository
    {
        Task<PaginationData<Restaurant>> GetRestaurants(string search, bool isAdmin = false, PaginationData<Restaurant> paginationData = null);
        Task<List<Restaurant>> GetRestaurantsAdmin();
        Task<Restaurant> GetRestaurant(string restaurantId);
        Task<bool> AddRestaurant(Restaurant newRestaurant);
        Task<bool> RemoveRestaurant(string restaurantId);
        Task<bool> UpdateRestaurant(string restaurantId, Restaurant restaurant);
    }
}