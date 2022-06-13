using BLL.Services;
using BLL.Validation;
using CardFileApi.JWT;
using CardFileApi.Logging;
using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _userService;
        private readonly JwtHandler _jwtHandler;
        private readonly ILoggerManager _logger;

        public AuthController(AuthService userService,
            UserManager<User> userManager,
            JwtHandler jwtHandler,
            ILoggerManager logger)
        {
            _userService = userService;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LogIn(UserLoginDTO user)
        {
            if (user == null)
            {
                _logger.LogInfo("No credentials were provided");
                return BadRequest("No credentials were provided");
            }

            try
            {
                var foundUser = await _userService.LogInAsync(user);

                var claims = await _jwtHandler.GetClaims(foundUser);
                var signingCredentials = _jwtHandler.GetSigningCredentials();
                var token = _jwtHandler.GenerateToken(signingCredentials, claims);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            catch (Exception e)
            {
                _logger.LogInfo(e.Message);
                return Unauthorized(e.Message);
            }
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(UserRegisterDTO user)
        {
            if (user == null)
            {
                _logger.LogInfo("No credentials were provided");
                return BadRequest("No credentials were provided");
            }

            try
            {
                var newUser = await _userService.SignUpAsync(user);

                return Ok(newUser);
            }
            catch (Exception e)
            {
                _logger.LogInfo(e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
