using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CardFileApi.JWT
{
    /// <summary>
    /// Class helper for generating a Jwt token
    /// </summary>
    public class JwtHandler: IJwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettigns;
        private readonly UserManager<User> _userManager;

        /// <summary>
        /// Constructor which accepts configuration class and UserManager
        /// </summary>
        /// <param name="configuration">Instance of class that implements IConfiguration interface to have acces to configuration in appsettings.json</param>
        /// <param name="userManager">Instance of UserManager class to work with users</param>
        public JwtHandler(IConfiguration configuration,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _jwtSettigns = _configuration.GetSection("Jwt");
            _userManager = userManager;
        }

        /// <summary>
        /// Generates signing credentials using configuration from appsettings.json
        /// </summary>
        /// <returns>Generated signing credentials</returns>
        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettigns.GetSection("Key").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        /// <summary>
        /// Creates claims for specified user
        /// </summary>
        /// <param name="user">Instance of User class whose claims are to be generated</param>
        /// <returns>List of genereated claims of the user</returns>
        public async Task<List<Claim>> GetClaims(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim(JwtRegisteredClaimNames.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var userRoles = await _userManager.GetRolesAsync(user);

            foreach (var role in userRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }

            return claims;
        }

        /// <summary>
        /// Generated a Jwt token using signing credentials along with list of claims
        /// </summary>
        /// <param name="signingCredentials">Instance of SigningCredentials class</param>
        /// <param name="claims">List of claims of the user</param>
        /// <returns>Generated Jwt token</returns>
        public JwtSecurityToken GenerateToken(SigningCredentials signingCredentials,
            List<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _jwtSettigns.GetSection("Issuer").Value,
                audience: _jwtSettigns.GetSection("Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: signingCredentials);

            return token;
        }
    }
}

