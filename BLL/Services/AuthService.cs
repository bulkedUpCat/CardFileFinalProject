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
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor that accepts UserManager to work with users, SignInManager to sign in the user, emailSender to send emails
        /// </summary>
        /// <param name="userManager">Instance of class UserManager to perform various operations on users</param>
        /// <param name="signInManager">Instance of class SignInManager to perform various operations regardign signing in a user</param>
        /// <param name="emailSender">Instance of class that implements IEmailSender interface to work with email</param>
        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailSender emailSender,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Logs the user in if valid credentials are provided and user is not banned
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

            var userBan = await _unitOfWork.BanRepository.GetByUserIdAsync(foundUser.Id);

            if (userBan != null && userBan.Expires > DateTime.Now)
            {
                throw new CardFileException($"User with email {foundUser.Email} is banned till {userBan.Expires.ToString("MM/dd/yyyy")}");
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

            await SendConfirmationLink(newUser.Email);

            return newUser;
        }

        /// <summary>
        /// Sends confirmation link on user's email
        /// </summary>
        /// <param name="user">User model to generate an email token</param>
        /// <param name="email">Email to receive a message</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task SendConfirmationLink(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with email {email}");
            }

            if (user.EmailConfirmed)
            {
                throw new CardFileException($"Email {email} is already confirmed");
            }

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var parameters = new Dictionary<string, string>
            {
                {"token", token },
                {"email", email }
            };

            var confirmationLink = QueryHelpers.AddQueryString("http://localhost:4200/confirm-email", parameters);

            try
            {
                _emailSender.SendSmtpMail(new EmailTemplate()
                {
                    To = user.Email,
                    Subject = "Email confirmation on Text Materials website",
                    Body = $"Click this link to confirm your email:\n{confirmationLink}\n\nIf it wasn't you, ignore this email please."
                });
            }
            catch (CardFileException e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Confirms email of the user
        /// </summary>
        /// <param name="token">Email confirmation token</param>
        /// <param name="email">Email of the user</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task ConfirmEmail(ConfirmEmailDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with email {model.Email}");
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);

            if (!result.Succeeded)
            {
                throw new CardFileException("Invalid email confirmation token");
            }
        }

        /// <summary>
        /// Changes the user name of the user by its id
        /// </summary>
        /// <param name="userId">Id of the user to update</param>
        /// <param name="model">Model that contains new user name of the user</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task ChangeUserName(string userId, ChangeUserNameDTO model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var userNameOccupied = await _userManager.FindByNameAsync(model.NewUserName);

            if (userNameOccupied != null)
            {
                throw new CardFileException($"User with user name {model.NewUserName} already exists");
            }

            try
            {
                user.UserName = model.NewUserName;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new CardFileException("Failed to change user name");
                }
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Changes the email of the user by its id and sends email confirmation link
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="model">New email of the user</param>
        /// <returns>Task representing an asynchronous operation</returns>
        public async Task ChangeEmail(string userId, ChangeEmailDTO model)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var emailOccupied = await _userManager.FindByEmailAsync(model.NewEmail);

            if (emailOccupied != null)
            {
                throw new CardFileException($"User with email {model.NewEmail} already exists");
            }

            try
            {
                user.Email = model.NewEmail;
                user.EmailConfirmed = false;
                var result = await _userManager.UpdateAsync(user);

                if (!result.Succeeded)
                {
                    throw new CardFileException("Failed to change email");
                }

                await SendConfirmationLink(user.Email);
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
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
