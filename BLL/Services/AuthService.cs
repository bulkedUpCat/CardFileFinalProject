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
    public class AuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public AuthService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleIdentity)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _roleManager = roleIdentity;
            _signInManager = signInManager;

        }

        public async Task<User> LogInAsync(UserLoginDTO user)
        {
            var foundUser = await _userManager.FindByEmailAsync(user.Email);

            if (foundUser == null)
            {
                throw new Exception($"User with email {user.Email} does not exist");
            }

            var result = await _signInManager.PasswordSignInAsync(foundUser, user.Password, false, false);

            if (!result.Succeeded)
            {
                throw new CardFileException($"Failed to log in with email {user.Email}");
            }

            return foundUser;
        }

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
