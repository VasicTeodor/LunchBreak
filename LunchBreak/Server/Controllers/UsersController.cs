using System;
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
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
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
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> GetUsers([FromQuery]PaginationDataInfo paginationInfo)
        {
            var pagination = _mapper.Map<PaginationData<User>>(paginationInfo);

            var result = await _userRepository.GetUsers(pagination);

            if(result.Items != null)
            {
                var resultToReturn = _mapper.Map<List<UserRegisterDTO>>(result.Items);

                return Ok(new GetUsers() { Successful = true, Users = resultToReturn, PaginationInfo = _mapper.Map<PaginationDataInfo>(result) });
            }
            else
            {
                return BadRequest(new GetUsers() { Successful = false, Error = "Failed to fetch all users" });
            }
        }

        [HttpPut]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
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

        [HttpPut]
        [Route("uploadprofileimage")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> UploadProfileImage([FromQuery]string userId, PictureData picture)
        {
            var user = await _userRepository.GetUser(userId);

            if(user == null)
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to upload user profile picture" });

            user.ProfilePicture.Data = Convert.ToBase64String(picture.Data);
            user.ProfilePicture.Type = picture.Type;

            var result = await _userRepository.UpdateUser(userId, user);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to update user profile" });
            }
        }

        [HttpPut]
        [Route("uploaddocumentimage")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> UploadDocumentImage([FromQuery]string userId, PictureData picture)
        {
            var user = await _userRepository.GetUser(userId);

            if (user == null)
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to upload users document picture" });

            user.DocumentPicture.Data = Convert.ToBase64String(picture.Data);
            user.DocumentPicture.Type = picture.Type;

            var result = await _userRepository.UpdateUser(userId, user);

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
        [Authorize(Policy = HelperAuth.Constants.Policy.Admin)]
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