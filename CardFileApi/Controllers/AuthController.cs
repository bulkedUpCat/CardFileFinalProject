using BLL.Abstractions.cs.Interfaces;
using BLL.Services;
using BLL.Validation;
using CardFileApi.JWT;
using CardFileApi.Logging;
using Core.DTOs;
using Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthService _authService;
        private readonly JwtHandler _jwtHandler;
        private readonly ILoggerManager _logger;
        private readonly IEmailSender _emailSender;

        public AuthController(AuthService authService,
            UserManager<User> userManager,
            JwtHandler jwtHandler,
            ILoggerManager logger,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _authService = authService;
            _jwtHandler = jwtHandler;
            _logger = logger;
            _emailSender = emailSender;
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
                var foundUser = await _authService.LogInAsync(user);

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
                var newUser = await _authService.SignUpAsync(user);

                return Ok(newUser);
            }
            catch (Exception e)
            {
                _logger.LogInfo(e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("forgotPassword")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest($"Failed to find a user with email {model.Email}");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var parameters = new Dictionary<string, string>
            {
                {"token", token },
                {"email", model.Email }
            };

            var callback = QueryHelpers.AddQueryString(model.ClientURI, parameters);

            try
            {
                _emailSender.SendSmtpMail(new EmailTemplate()
                {
                    To = user.Email,
                    Subject = "Password reset on Text Materials website",
                    Body = $"Click this link to reset your password:\n{callback}\n\nIf it wasn't you, ignore this email please.",
                    UserId = user.Id
                });
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }

            return Ok();
        }

        [HttpPost("passwordReset")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            if (!ModelState.IsValid || model.Password != model.ConfirmPassword)
            {
                return BadRequest("Model state not valid");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (string.IsNullOrEmpty(model.Token))
            {
                return BadRequest("Empty password reset token");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest("Failed to reset the password");
            }

            return Ok();
        }
    }
}
