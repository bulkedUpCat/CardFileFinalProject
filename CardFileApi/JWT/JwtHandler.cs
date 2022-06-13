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
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _jwtSettigns;
        private readonly UserManager<User> _userManager;

        public JwtHandler(IConfiguration configuration,
            UserManager<User> userManager)
        {
            _configuration = configuration;
            _jwtSettigns = _configuration.GetSection("Jwt");
            _userManager = userManager;
        }

        public SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(_jwtSettigns.GetSection("Key").Value);
            var secret = new SymmetricSecurityKey(key);

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

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

