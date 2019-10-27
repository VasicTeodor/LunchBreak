using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LunchBreak.Helpers;
using LunchBreak.Infrastructure.Entities;
using LunchBreak.Infrastructure.Interfaces;
using LunchBreak.Server.Services;
using LunchBreak.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace LunchBreak.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthorizationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public AuthorizationController(IMapper mapper, IUserRepository userRepository, IConfiguration configuration)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _configuration = configuration;
        }
        
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegisterDTO user)
        {
            var newUser = _mapper.Map<User>(user);
            newUser.ProfilePicture = new Image() { };
            newUser.DocumentPicture = new Image() { };
            newUser.Role = HelperAuth.Constants.Policy.User;

            if (await _userRepository.CheckIfUserExists(user.Username))
            {
                return BadRequest(new RegisterResult() { Successful = false, Errors = new List<string>() { "Username is already taken." } });
            }

            newUser.Password = AuthHashService.GeneratePasswordHash(user.Password);

            var result = await _userRepository.AddUser(newUser);

            if (result)
            {
                return Ok(new RegisterResult(){Successful = true});
            }
            else
            {
                return BadRequest(new RegisterResult(){Successful = true, Errors = new List<string>(){"Failed to register."}});
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(LoginData loginData)
        {
            var user = await _userRepository.GetUserByUsername(loginData.Username);

            if (user == null)
            {
                return BadRequest(new LoginResult() {Successful = false, Error = "Wrong username"});
            }

            if (!AuthHashService.CheckPassword(loginData.Password, user.Password))
            {
                return BadRequest(new LoginResult() { Successful = false, Error = "Wrong username" });
            }

            var listOfRole = new List<HelperAuth.RoleTypeEnum>() {
                HelperAuth.RoleTypeEnum.User
            };

            if (user != null)
            {

                if (user.Role.Equals(HelperAuth.Constants.Policy.Admin))
                {
                    listOfRole.Add(HelperAuth.RoleTypeEnum.Editor);
                    listOfRole.Add(HelperAuth.RoleTypeEnum.Admin);
                }

                if (user.Role.Equals(HelperAuth.Constants.Policy.Editor))
                {
                    listOfRole.Add(HelperAuth.RoleTypeEnum.Editor);
                }
            }

            var userIdentity = HelperAuth.GenerateClaimsIdentity(user.Id, user.Username, listOfRole);
            var tokenValiditi = _configuration.GetSection("TokenValidityInMinutes").Value;
            var issuer = _configuration.GetSection("Issuer").Value;
            var secret = _configuration.GetSection("Secret").Value;
            var jwtOptions = new HelperAuth.JwtOptions(int.Parse(tokenValiditi))
            {
                Issuer = issuer,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), SecurityAlgorithms.HmacSha256)
            };

            var retJwt = HelperAuth.GenerateJwt(userIdentity, jwtOptions);
            return Ok(retJwt);
        }
    }
}