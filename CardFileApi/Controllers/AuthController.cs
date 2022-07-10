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

        /// <summary>
        /// Constructor with accepts services for working with authentication and authorization, Jwt helper for generating a Jwt token
        /// </summary>
        /// <param name="authService">Instance of class that implements IAuthService interface for working with authentication and authorization</param>
        /// <param name="jwtHandler">Instance of class JWtHandler which contains logic to create a Jwt token</param>
        public AuthController(IAuthService authService,
            IJwtHandler jwtHandler)
        {
            _authService = authService;
            _jwtHandler = jwtHandler;
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

        /// <summary>
        /// Signs up the user and returns the user entity
        /// </summary>
        /// <param name="user">Data transfer object which contains data for a new user</param>
        [HttpPost("signup")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SignUp(UserRegisterDTO user)
        {
            var newUser = await _authService.SignUpAsync(user);

            return Ok(newUser);
        }

        [HttpPost("sendConfirmationLink")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendEmailConfirmationLink(SendEmailConfirmationLinkDTO model) 
        {
            await _authService.SendConfirmationLink(model.Email);

            return Ok("Email confirmation link sent");
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
            await _authService.ConfirmEmail(model);

            return Ok("Email confirmed");
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
            await _authService.ForgotPassword(model);

            return Ok("Password reset link sent");
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
            await _authService.ResetPassword(model);

            return Ok("Password reset");
        }

        /// <summary>
        /// Changes the user name of the user with given id to the new specified one
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="model">New user name for the user</param>
        [HttpPut("{id}/userName")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeUserName(string id, [FromBody] ChangeUserNameDTO model)
        {
            await _authService.ChangeUserName(id, model);

            return Ok("User name changed");
        }

        /// <summary>
        /// Changes the email of the user with given id to the new specified email
        /// </summary>
        /// <param name="id">Id of the user to update</param>
        /// <param name="model">New email for the user</param>
        [HttpPut("{id}/email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType (StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ChangeEmail(string id, [FromBody] ChangeEmailDTO model)
        {
            await _authService.ChangeEmail(id, model);

            return Ok("Email changed");
        }
    }
}
