using BLL.Abstractions.cs.Interfaces;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using DAL.Abstractions.Interfaces;
using Microsoft.AspNetCore.Identity;
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

        /// <summary>
        /// Constructor that takes two arguments
        /// </summary>
        /// <param name="userManager">Instance of class UserManager to perform various operations on users</param>
        /// <param name="signInManager">Instance of class SignInManager to perform various operations regardign signing in a user</param>
        public AuthService(UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;

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
                throw new CardFileException($"User with email {user.Email} does not exist");
            }

            var result = await _signInManager.PasswordSignInAsync(foundUser, user.Password, false, false);

            if (!result.Succeeded)
            {
                throw new CardFileException($"Failed to log in with email {user.Email}");
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
            var userExists = await _userManager.FindByEmailAsync(user.Email);

            if (userExists != null)
            {
                throw new CardFileException($"User with specified email {user.Email} already exists in database");
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
    }
}
