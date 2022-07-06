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
using System.Net.Mime;
using System.Threading.Tasks;

namespace CardFileApi.Controllers
{
    /// <summary>
    /// Controller that provides endpoints for working with authorization and authentication
    /// </summary>
    [ApiVersion("1.0")]
    [ApiController]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IJwtHandler _jwtHandler;
        private readonly ILoggerManager _logger;

        /// <summary>
        /// Constructor with accepts services for working with authentication and authorization, Jwt helper for generating a Jwt token and a logger to log information
        /// </summary>
        /// <param name="authService">Instance of class that implements IAuthService interface for working with authentication and authorization</param>
        /// <param name="jwtHandler">Instance of class JWtHandler which contains logic to create a Jwt token</param>
        /// <param name="logger">Instance of class that implements ILoggerManager interface to log information</param>
        public AuthController(IAuthService authService,
            IJwtHandler jwtHandler,
            ILoggerManager logger)
        {
            _authService = authService;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        /// <summary>
        /// Logs the user in and returns a generated Jwt token
        /// </summary>
        /// <param name="user">Data transfer object which contains data of the user</param>
        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> LogIn(UserLoginDTO user)
        {
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

        /// <summary>
        /// Signs up the user and returns the user entity
        /// </summary>
        /// <param name="user">Data transfer object which contains data for a new user</param>
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp(UserRegisterDTO user)
        {
            try
            {
                var newUser = await _authService.SignUpAsync(user);

                return Ok(newUser);
            }
            catch (Exception e)
            {
                _logger.LogInfo($"While signing up: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Confirms email of the user
        /// </summary>
        /// <param name="model">Model that contains user's email and email confirmation token</param>
        [HttpPost("confirmEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmail(ConfirmEmailDTO model)
        {
            try
            {
                await _authService.ConfirmEmail(model);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogInfo($"While confirming email: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Sends a reset password link on user's email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("forgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDTO model)
        {
            try
            {
                await _authService.ForgotPassword(model);

                return Ok();
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"While sending a reset password link: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Resets the password of the specified user
        /// </summary>
        /// <param name="model">Data transfer object which contains data of the user whose password needs to be reset</param>
        [HttpPost("passwordReset")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO model)
        {
            try
            {
                await _authService.ResetPassword(model);

                return Ok();
            }
            catch (CardFileException e)
            {
                _logger.LogInfo($"While resetting the password: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Changes the user name of the user with given id to the new specified one
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <param name="model">New user name for the user</param>
        [HttpPut("{id}/userName")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUserName(string id, [FromBody] ChangeUserNameDTO model)
        {
            try
            {
                await _authService.ChangeUserName(id, model);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}/email")]
        public async Task<IActionResult> ChangeEmail(string id, [FromBody] ChangeEmailDTO model)
        {
            try
            {
                await _authService.ChangeEmail(id, model);

                return NoContent();
            }
            catch (CardFileException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
