using LunchBreak.Shared.Models;
using System.Collections.Generic;

namespace LunchBreak.Shared
{
    public class LoginResult
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
        public string Token { get; set; }
        public bool ApprovedAccount { get; set; }
        public string Id { get; set; }
        public int ExpiresIn { get; set; }
        public string Username { get; set; }
    }

    public class RegisterResult
    {
        public bool Successful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class OperationSuccessResponse
    {
        public bool Successful { get; set; }
        public string Error { get; set; }
    }

    public class GetLunch
    {
        public bool Successful { get; set; }
        public LunchDto Lunch { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class GetLunches
    {
        public bool Successful { get; set; }
        public PaginationDataInfo PaginationInfo { get; set; }
        public List<LunchDto> Lunches { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class GetRestaurant
    {
        public bool Successful { get; set; }
        public RestaurantDto Restaurant { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }

    public class GetRestaurants
    {
        public bool Successful { get; set; }
        public PaginationDataInfo PaginationInfo { get; set; }
        public List<RestaurantDto> Restaurants { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
    public class GetUser
    {
        public bool Successful { get; set; }
        public UserRegisterDTO User { get; set; }
        public string Error { get; set; }
    }

    public class GetUsers
    {
        public bool Successful { get; set; }
        public PaginationDataInfo PaginationInfo { get; set; }
        public List<UserRegisterDTO> Users { get; set; }
        public string Error { get; set; }
    }

    public class GetStats
    {
        public bool Successful { get; set; }
        public int LunchesNum { get; set; }
        public int RestaurantsNum { get; set; }
        public int UsersNum { get; set; }
        public string Error { get; set; }
    }
}