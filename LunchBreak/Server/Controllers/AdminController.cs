using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
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

        //[HttpGet]
        //[Route("restaurants")]
        //[Authorize(Policy = HelperAuth.Constants.Policy.User)]
        //public async Task<IActionResult> Get()
        //{
        //    throw new NotImplementedException();
        //}

        //[HttpPost]
        //[Authorize(Policy = HelperAuth.Constants.Policy.User)]
        //public async Task<IActionResult> Post(Restaurant restaurant)
        //{

        //    throw new NotImplementedException();
        //}

        [HttpPut]
        [Route("approverestaurant/{restaurantId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> ApproveRestaurant(string restaurantId)
        {
            var restaurant = await _restaurantRepository.GetRestaurant(restaurantId);
            if(restaurant != null)
            {
                restaurant.Approved = true;
                var result = await _restaurantRepository.UpdateRestaurant(restaurantId, restaurant);

                if (result)
                {
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
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> ApproveLunch(string lunchId)
        {
            var lunch = await _lunchRepository.GetLunch(lunchId);
            if (lunch != null)
            {
                lunch.Approved = true;
                var result = await _lunchRepository.UpdateLunch(lunchId, lunch);

                if (result)
                {
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
        [Route("approvecomment/{data}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> ApproveComment(string data)
        {
            var restaurantId = data.Split('+')[0];
            var commentId = data.Split('+')[1];

            var restaurant = await _restaurantRepository.GetRestaurant(restaurantId);
            if (restaurant != null)
            {
                var comment = restaurant.Comments.FirstOrDefault(c => c.Id == commentId);
                comment.Approved = true;

                restaurant.Comments.Remove(restaurant.Comments.FirstOrDefault(c => c.Id == commentId));
                restaurant.Comments.Add(comment);

                var result = await _restaurantRepository.UpdateRestaurant(restaurantId, restaurant);

                if (result)
                {
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

        //[HttpDelete]
        //[Route("remove/{restaurantId}")]
        //[Authorize(Policy = HelperAuth.Constants.Policy.User)]
        //public async Task<IActionResult> Delete(string restaurantId)
        //{
        //    throw new NotImplementedException();
        //}
    }
}