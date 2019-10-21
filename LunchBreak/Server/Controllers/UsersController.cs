﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using LunchBreak.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LunchBreak.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UsersController(IMapper mapper, IUserRepository userRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
        }
        
        [HttpGet]
        [Route("user/{userId}")]
        public async Task<IActionResult> GetUser(string userId)
        {
            var result = await _userRepository.GetUser(userId);

            if (result != null)
            {
                var resultToReturn = _mapper.Map<UserRegisterDTO>(result);
                return Ok(new GetUser() { Successful = true, User = resultToReturn });
            }
            else
            {
                return BadRequest(new GetUser() { Successful = false, Error = "Failed to fetch user" });
            }
        }

        [HttpGet]
        [Route("allusers")]
        public async Task<IActionResult> GetUsers()
        {
            var result = await _userRepository.GetUsers();

            if(result != null)
            {
                var resultToReturn = _mapper.Map<List<UserRegisterDTO>>(result);
                return Ok(new GetUsers() { Successful = true, Users = resultToReturn });
            }
            else
            {
                return BadRequest(new GetUsers() { Successful = false, Error = "Failed to fetch all users" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            var result = await _userRepository.UpdateUser(user.Id, user);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to update user profile" });
            }
        }

        [HttpDelete]
        [Route("remove/{userId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Delete(string userId)
        {
            var result = await _userRepository.RemoveUser(userId);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to update restaurant" });
            }
        }
    }
}