using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using LunchBreak.Server.Services;
using LunchBreak.Shared;
using LunchBreak.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LunchBreak.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;
        private readonly ILunchRepository _lunchRepository;
        private readonly IUserRepository _userRepository;

        public AdminController(IMapper mapper, IRestaurantRepository restaurantRepository, ILunchRepository lunchRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
            _lunchRepository = lunchRepository;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Route("getstatsupdate")]
        [Authorize(Policy = HelperAuth.Constants.Policy.Admin)]
        public async Task<IActionResult> Get()
        {
            var lunches = await _lunchRepository.GetLunchesAdmin();
            var restaurants = await _restaurantRepository.GetRestaurantsAdmin();
            var users = await _userRepository.GetUsersAdmin();
            var response = new GetStats() { Successful = true };

            if(lunches != null)
            {
                response.LunchesNum = lunches.Count;
            }
            else
            {
                response.LunchesNum = 0;
            }

            if (restaurants != null)
            {
                response.RestaurantsNum = restaurants.Count;
            }
            else
            {
                response.RestaurantsNum = 0;
            }

            if (users != null)
            {
                response.UsersNum = users.Count;
            }
            else
            {
                response.UsersNum = 0;
            }

            return Ok(response);
        }

        [HttpPut]
        [Route("approverestaurant/{restaurantId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.Admin)]
        public async Task<IActionResult> ApproveRestaurant(string restaurantId)
        {
            var restaurant = await _restaurantRepository.GetRestaurant(restaurantId);
            if(restaurant != null)
            {
                restaurant.Approved = true;
                var result = await _restaurantRepository.UpdateRestaurant(restaurantId, restaurant);

                if (result)
                {
                    //EmailService.SendEmail("LunchBreak: Restaurant Approved", $"Restaurant that you added with name: {restaurant.Name} is approved by admin and now all users can see it.", "usermail");
                    return Ok(new OperationSuccessResponse() { Successful = true });
                }
                else
                {
                    return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while updating restaurnat status" });
                }
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while approving restaurant" });
            }
        }

        [HttpPut]
        [Route("approvelunch/{lunchId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.Admin)]
        public async Task<IActionResult> ApproveLunch(string lunchId)
        {
            var lunch = await _lunchRepository.GetLunch(lunchId);
            if (lunch != null)
            {
                lunch.Approved = true;
                var result = await _lunchRepository.UpdateLunch(lunchId, lunch);

                if (result)
                {
                    //EmailService.SendEmail("LunchBreak: Lunch Approved", $"Lunch that you added with name: {lunch.Name} is approved by admin and now all users can see it.", "usermail");
                    return Ok(new OperationSuccessResponse() { Successful = true });
                }
                else
                {
                    return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while updating lunch status" });
                }
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while approving lunch" });
            }
        }

        [HttpPut]
        [Route("approveuser")]
        [Authorize(Policy = HelperAuth.Constants.Policy.Admin)]
        public async Task<IActionResult> ApproveUser([FromQuery]string userId, [FromQuery]bool approve)
        {
            var user = await _userRepository.GetUser(userId);
            if (user != null)
            {
                user.Approved = approve;
                var result = await _userRepository.UpdateUser(userId, user);

                if (result && approve)
                {
                    //EmailService.SendEmail("LunchBreak: Account Approved", $"Congrats, your account is approved by our admin team, you can start using it.", "usermail");
                    return Ok(new OperationSuccessResponse() { Successful = true });
                }
                else if(result && !approve)
                {
                    //EmailService.SendEmail("LunchBreak: Account Not Approved", $"Sorry, your account is not approved by our admin team, check your data again.", "usermail");
                    return Ok(new OperationSuccessResponse() { Successful = false });
                }
                else
                {
                    return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while updating lunch status" });
                }
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while approving lunch" });
            }
        }

        [HttpPut]
        [Route("approvecomment/{data}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.Admin)]
        public async Task<IActionResult> ApproveComment(string data)
        {
            var restaurantId = data.Split('+')[0];
            var commentId = data.Split('+')[1];

            var restaurant = await _restaurantRepository.GetRestaurant(restaurantId);
            if (restaurant != null)
            {
                var comment = restaurant.Comments.FirstOrDefault(c => c.Id == commentId);
                var index = restaurant.Comments.IndexOf(comment);
                comment.Approved = true;

                restaurant.Comments.Remove(restaurant.Comments.FirstOrDefault(c => c.Id == commentId));
                restaurant.Comments.Insert(index, comment);

                restaurant.Grade = CalculateRestaurantGrade(restaurant);

                var result = await _restaurantRepository.UpdateRestaurant(restaurantId, restaurant);

                if (result)
                {
                    //EmailService.SendEmail("LunchBreak: Comment Approved", $"Your comment for restaurant: {restaurant.Name} is approved by our admin team, everyone can now see it.", "usermail");
                    return Ok(new OperationSuccessResponse() { Successful = true });
                }
                else
                {
                    return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while updating lunch status" });
                }
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Error while approving lunch" });
            }
        }

        private int CalculateRestaurantGrade(Restaurant restaurant)
        {
            int counter = 0;
            int gradeSum = 0;

            if (restaurant.Comments != null)
            {
                foreach (var comment in restaurant.Comments)
                {
                    if (comment.Approved)
                    {
                        gradeSum += comment.Grade;
                        counter++;
                    }
                }
            }

            if (counter != 0)
            {
                return gradeSum / counter;
            }
            else
            {
                return 0;
            }
        }
    }
}