using LunchBreak.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LunchBreak.Server.Extensions
{
    public static class Extensions
    {
        public static bool IsAdmin(this ClaimsPrincipal user)
        {
            var retVal = false;
            var isAdmin = user.Claims.FirstOrDefault(c => c.Type.Equals("rol_Admin"))?.Value;

            if (isAdmin != null)
            {
                if (bool.Parse(isAdmin))
                    retVal = true;
            }

            return retVal;
        }

        public static async Task<bool> UserApproved(this ClaimsPrincipal user, IUserRepository userRepository)
        {
            var retVal = false;
            var userId = user.Claims.FirstOrDefault(c => c.Type.Equals("id"))?.Value;

            if (userId != null)
            {
                if (await userRepository.UserIsApproved(userId))
                    retVal = true;
            }

            return retVal;
        }
    }
}
