using BLL.Abstractions.cs.Interfaces;
using BLL.Email;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Service to perform various operation regarding authentication and authorization such as logging in, signing up
    /// </summary>
    public class AuthService: IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        /// <summary>
        /// Constructor that takes three arguments
        /// </summary>
        /// <param name="userManager">Instance of class UserManager to perform various operations on users</param>
        /// <param name="signInManager">Instance of class SignInManager to perform various operations regardign signing in a user</param>
        /// <param name="emailSender">Instance of class that implements IEmailSender interface to work with email</param>
        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Logs the user in if valid credentials are provided
        /// </summary>
        /// <param name="user">Credentials: email and password</param>
        /// <returns>User if credentials were valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<User> LogInAsync(UserLoginDTO user)
        {
            var foundUser = await _userManager.FindByEmailAsync(user.Email);

            if (foundUser == null)
            {
                throw new CardFileException("Wrong email or password");
            }

            var result = await _signInManager.PasswordSignInAsync(foundUser, user.Password, false, false);

            if (!result.Succeeded)
            {
                throw new CardFileException("Wrong email or password");
            }

            return foundUser;
        }

        /// <summary>
        /// Signs the user up if valid credentials were provided
        /// </summary>
        /// <param name="user">Sign up model: name, email, password, confirm password</param>
        /// <returns>User if credentials were valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<User> SignUpAsync(UserRegisterDTO user)
        {
            var emailExists = await _userManager.FindByEmailAsync(user.Email);

            if (emailExists != null)
            {
                throw new CardFileException($"User with email {user.Email} already exists in database");
            }

            var userNameExists = await _userManager.FindByNameAsync(user.Name);

            if (userNameExists != null)
            {
                throw new CardFileException($"User with name {user.Name} already exists in database");
            }

            if (user.Password != user.ConfirmPassword)
            {
                throw new CardFileException($"Passwords don't match");
            }

            var newUser = new User()
            {
                UserName = user.Name,
                Email = user.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (!result.Succeeded)
            {
                throw new CardFileException($"Failed to create a user with email {user.Email}");
            }

            return newUser;
        }

        /// <summary>
        /// Sends a reset password link on user's email 
        /// </summary>
        /// <param name="model">Model that contains data about the user</param>
        /// <returns>Task representing an asynchronous operation</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task ForgotPassword(ForgotPasswordDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with email {model.Email}");
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
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Resets user's password
        /// </summary>
        /// <param name="model">Model that contains data of the user and the new password</param>
        /// <returns>Task representing an asynchronous operation</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task ResetPassword(ResetPasswordDTO model)
        {
            if (model.Password != model.ConfirmPassword)
            {
                throw new CardFileException("Passwords don't match");
            }

            if (string.IsNullOrWhiteSpace(model.Token))
            {
                throw new CardFileException("Empty password reset token");
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new CardFileException("User not found");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (!result.Succeeded)
            {
                throw new CardFileException("Failed to reset the password");
            }
        }
    }
}
