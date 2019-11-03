using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using LunchBreak.Server.Extensions;
using LunchBreak.Shared;
using LunchBreak.Shared.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LunchBreak.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LunchBreakController : ControllerBase
    {
        private readonly ILunchRepository _lunchRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public LunchBreakController(ILunchRepository lunchRepository, IUserRepository userRepository, IMapper mapper)
        {
            _lunchRepository = lunchRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("lunch/{lunchId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Get(string lunchId)
        {
            var result = await _lunchRepository.GetLunch(lunchId);

            if (result != null)
            {
                var resultToReturn = _mapper.Map<LunchDto>(result);
                return Ok(new GetLunch() { Successful = true, Lunch = resultToReturn });
            }
            else
            {
                return BadRequest(new GetLunch() { Successful = false, Errors = new List<string>() { "Error while fetching lunch" } });
            }
        }

        [HttpGet]
        [Route("lunches")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Get([FromQuery]PaginationDataInfo paginationInfo, [FromQuery]string search = "", [FromQuery]bool isAdmin = false)
        {
            var pagination = _mapper.Map<PaginationData<Lunch>>(paginationInfo);

            var result = await _lunchRepository.GetLunches(search, isAdmin, pagination);
            
            if (result.Items != null)
            {
                var resultToReturn = _mapper.Map<List<LunchDto>>(result.Items);

                return Ok(new GetLunches() { Successful = true, Lunches = resultToReturn, PaginationInfo = _mapper.Map<PaginationDataInfo>(result)});
            }
            else
            {
                return BadRequest(new GetLunches() { Successful = false, Errors = new List<string>() { "Error while fetching lunches" } });
            }
        }

        [HttpPost]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Post(Lunch lunch)
        {
            if (!(await User.UserApproved(_userRepository)))
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "User not approved" });
            }

            if (lunch.IsPublic == "Private")
            {
                var user = await _userRepository.GetUser(lunch.CreatedBy);
                if (!string.IsNullOrEmpty(user.TeamId))
                {
                    lunch.TeamId = user.TeamId;
                }
            }

            lunch.Approved = false;

            var result = await _lunchRepository.AddLunch(lunch);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to add new lunch break" });
            }
        }

        [HttpPut]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Update(Lunch lunch)
        {
            if (!(await User.UserApproved(_userRepository)))
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "User not approved" });
            }

            lunch.TotalPrice = CalculateTotalPrice(lunch);
            var result = await _lunchRepository.UpdateLunch(lunch.Id, lunch);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to update lunch break" });
            }
        }

        [HttpDelete]
        [Route("remove/{lunchId}")]
        [Authorize(Policy = HelperAuth.Constants.Policy.User)]
        public async Task<IActionResult> Delete(string lunchId)
        {
            if (!(await User.UserApproved(_userRepository)))
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "User not approved" });
            }

            var result = await _lunchRepository.RemoveLunch(lunchId);

            if (result)
            {
                return Ok(new OperationSuccessResponse() { Successful = true });
            }
            else
            {
                return BadRequest(new OperationSuccessResponse() { Successful = false, Error = "Failed to remove selected lunch break" });
            }
        }

        private decimal CalculateTotalPrice(Lunch lunch)
        {
            decimal totalPrice = 0;

            if(lunch.Orders != null)
            {
                foreach (var order in lunch.Orders)
                {
                    totalPrice += order.Price;
                }
            }

            return totalPrice;
        }
    }
}