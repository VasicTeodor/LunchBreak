using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using LunchBreak.Shared;
using LunchBreak.Shared.Models;
using AutoMapper;
using LunchBreak.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace LunchBreak.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IRestaurantRepository _restaurantRepository;

        public RestaurantController(IMapper mapper, IRestaurantRepository restaurantRepository)
        {
            _mapper = mapper;
            _restaurantRepository = restaurantRepository;
        }

        [HttpGet]
        [Route("restaurant/{restaurantId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Get(string restaurantId)
        {
            var restaurant = await _restaurantRepository.GetRestaurant(restaurantId);

            if(restaurant != null)
            {
                var restaurantToReturn = _mapper.Map<RestaurantDto>(restaurant);
                return Ok(new GetRestaurant() { Successful = true, Restaurant = restaurantToReturn});
            }
            else
            {
                return BadRequest(new GetRestaurant() { Successful = false, Errors = new List<string>() { "Error while fetching restaurant" } });
            }
        }

        [HttpGet]
        [Route("restaurants")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Get([FromQuery]bool isAdmin = false)
        {
            var restaurants = await _restaurantRepository.GetRestaurants(isAdmin);

            if (restaurants != null)
            {
                var resultToReturn = _mapper.Map<List<RestaurantDto>>(restaurants);
                return Ok(new GetRestaurants() { Successful = true, Restaurants = resultToReturn });
            }
            else
            {
                return BadRequest(new GetRestaurants() { Successful = false, Errors = new List<string>() { "Error while fetching restaurants" } });
            }
        }

        [HttpPost]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Post(Restaurant restaurant)
        {

            restaurant.Approved = false;
            var result = await _restaurantRepository.AddRestaurant(restaurant);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to add new restaurant"});
            }
        }

        [HttpPut]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Update(Restaurant restaurant)
        {
            restaurant.Grade = CalculateRestaurantGrade(restaurant);
            var result = await _restaurantRepository.UpdateRestaurant(restaurant.Id, restaurant);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to update restaurant" });
            }
        }

        [HttpDelete]
        [Route("remove/{restaurantId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Delete(string restaurantId)
        {
            var result = await _restaurantRepository.RemoveRestaurant(restaurantId);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to update restaurant" });
            }
        }

        private int CalculateRestaurantGrade(Restaurant restaurant)
        {
            int counter = 0;
            int gradeSum = 0;

            if(restaurant.Comments != null)
            {
                foreach(var comment in restaurant.Comments)
                {
                    gradeSum += comment.Grade;
                    counter++;
                }
            }

            if(counter != 0)
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