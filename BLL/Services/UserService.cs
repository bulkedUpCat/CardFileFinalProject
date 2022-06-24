using AutoMapper;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using DAL.Abstractions.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class UserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedList<UserDTO>> GetAll(UserParameters userParameters)
        {
            var users = await _unitOfWork.UserRepository.GetWithDetailsAsync();
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            
            foreach(var user in userDTOs)
            {
                var foundUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(foundUser);
                user.Roles = roles?.ToList();
            }

            return PagedList<UserDTO>.ToPagedList(userDTOs, userParameters.PageNumber, userParameters.PageSize);
        }

        public async Task<UserDTO> GetUserById(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new CardFileException("Invalid id");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                throw new CardFileException($"User with id {id} doesn't exist");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var userDTO = _mapper.Map<UserDTO>(user);
            userDTO.Roles = roles?.ToList();

            return userDTO;
        }

        public async Task<UserDTO> ToggleReceiveNotifications(string userId, bool receiveNotifications)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            try
            {
                user.ReceiveNotifications = receiveNotifications;
                _unitOfWork.UserRepository.Update(user);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<UserDTO>(user);
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }
    }
}
