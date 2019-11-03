using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using LunchBreak.Shared;
using Microsoft.IdentityModel.Tokens;

namespace LunchBreak.Helpers
{
    public static class HelperAuth
    {
        public class Constants
        {
            public class Claims
            {
                public const string Rol = "rol";
                public const string Id = "id";
                public const string Name = "name";

                public class Format
                {
                    public const string RoleClaim = "{0}_{1}";
                }
            }

            public class Policy
            {
                public const string User = "User";
                public const string Editor = "Editor";
                public const string Admin = "Admin";
            }
        }

        public enum RoleTypeEnum
        {
            User = 1,
            Editor = 2,
            Admin = 3
        }

        public class JwtOptions
        {
            public JwtOptions(int validityMinutes)
            {
                ValidFor = TimeSpan.FromMinutes(validityMinutes);
            }

            /// <summary>
            /// 4.1.1.  "iss" (Issuer) Claim - The "iss" (issuer) claim identifies the principal that issued the JWT.
            /// </summary>
            public string Issuer { get; set; }

            /// <summary>
            /// 4.1.2.  "sub" (Subject) Claim - The "sub" (subject) claim identifies the principal that is the subject of the JWT.
            /// </summary>
            public string Subject { get; set; }

            /// <summary>
            /// 4.1.4.  "exp" (Expiration Time) Claim - The "exp" (expiration time) claim identifies the expiration time on or after which the JWT MUST NOT be accepted for processing.
            /// </summary>
            public DateTime Expiration => IssuedAt.Add(ValidFor);

            /// <summary>
            /// 4.1.5.  "nbf" (Not Before) Claim - The "nbf" (not before) claim identifies the time before which the JWT MUST NOT be accepted for processing.
            /// </summary>
            public DateTime NotBefore => DateTime.UtcNow;

            /// <summary>
            /// 4.1.6.  "iat" (Issued At) Claim - The "iat" (issued at) claim identifies the time at which the JWT was issued.
            /// </summary>
            public DateTime IssuedAt => DateTime.UtcNow;

            /// <summary>
            /// Set the timespan the token will be valid for (default is 120 min)
            /// </summary>
            public TimeSpan ValidFor { get; }

            /// <summary>
            /// "jti" (JWT ID) Claim (default ID is a GUID)
            /// </summary>
            public Func<string> JtiGenerator => () => Guid.NewGuid().ToString();

            /// <summary>
            /// The signing key to use when generating tokens.
            /// </summary>
            public SigningCredentials SigningCredentials { get; set; }

            /// <summary>
            /// The custom data of JWT
            /// </summary>
            public Dictionary<string, string> Payload { get; set; }
        }


        public static ClaimsIdentity GenerateClaimsIdentity(string id, string name, IList<RoleTypeEnum> roles)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(Constants.Claims.Id, id),
                new Claim(Constants.Claims.Name, name)
            };
            claims.AddRange(roles.Select((item) => new Claim(string.Format(Constants.Claims.Format.RoleClaim, Constants.Claims.Rol, item), "true")));

            return new ClaimsIdentity(new GenericIdentity(name, "Token"), claims);
        }

        public static LoginResult GenerateJwt(ClaimsIdentity identity, JwtOptions jwtIssuerOptions, bool approved)
        {
            var response = new LoginResult()
            {
                Id = identity.Claims.Single(c => c.Type == Constants.Claims.Id).Value,
                Token = GenerateEncodedToken(identity, jwtIssuerOptions),
                ExpiresIn = (int)jwtIssuerOptions.ValidFor.TotalSeconds,
                ApprovedAccount = approved,
                Username = identity.Claims.Single(c => c.Type == Constants.Claims.Name).Value,
                Successful = true
            };

            return response;
        }

        public static string UserClaim()
        {
            return string.Format(
                Constants.Claims.Format.RoleClaim,
                Constants.Claims.Rol, RoleTypeEnum.User);
        }

        public static string EditorClaim()
        {
            return string.Format(
                Constants.Claims.Format.RoleClaim,
                Constants.Claims.Rol, RoleTypeEnum.Editor);
        }

        public static string AdminClaim()
        {
            return string.Format(
                Constants.Claims.Format.RoleClaim,
                Constants.Claims.Rol, RoleTypeEnum.Admin);
        }

        private static string GenerateEncodedToken(ClaimsIdentity identity, JwtOptions jwtOptions)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Jti, jwtOptions.JtiGenerator()),
                new Claim(JwtRegisteredClaimNames.Iat, jwtOptions.IssuedAt.ToUnixEpochDate().ToString(), ClaimValueTypes.Integer64),
                identity.FindFirst(Constants.Claims.Id)
            };
            claims.AddRange(identity.FindAll(c => c.Type.StartsWith(Constants.Claims.Rol, StringComparison.OrdinalIgnoreCase)));

            // Create the JWT security token and encode it.
            var jwt = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                claims: claims,
                notBefore: jwtOptions.NotBefore,
                expires: jwtOptions.Expiration,
                signingCredentials: jwtOptions.SigningCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private static long ToUnixEpochDate(this DateTime date)
        {
            return (long)Math.Round((date.ToUniversalTime() - new DateTimeOffset(1970, 1, 1, 0, 0, 0, TimeSpan.Zero)).TotalSeconds);
        }

        public static string ReadUserId(this ClaimsPrincipal principal)
        {
            return principal.Claims.Single(c => c.Type == Constants.Claims.Id).Value;
        }
    }
}