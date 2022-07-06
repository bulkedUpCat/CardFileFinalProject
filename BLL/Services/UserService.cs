using AutoMapper;
using BLL.Abstractions.cs.Interfaces;
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
    /// <summary>
    /// Service to perform various operations on User entities such as getting all users from the database,
    /// getting a single user from the database by their id, changing the receive notifications status of the user by their id
    /// </summary>
    public class UserService: IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor that accepts unit of work to access repositories, UserManager to work with users and Mapper to map entities
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork interface</param>
        /// <param name="userManager">Instance of UserManager</param>
        /// <param name="mapper">Instance of class that implements IMapper interface</param>
        /// <param name="emailService">Instance of class that implements IEmailService</param>
        public UserService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// Finds all users by parameters
        /// </summary>
        /// <param name="userParameters">Parameters to take into account</param>
        /// <returns>All users that satisfy the parameters</returns>
        public async Task<PagedList<UserDTO>> GetAll(UserParameters userParameters)
        {
            var users = await _unitOfWork.UserRepository.GetWithDetailsAsync(userParameters);
            var userDTOs = _mapper.Map<IEnumerable<UserDTO>>(users);
            
            foreach(var user in userDTOs)
            {
                var foundUser = await _userManager.FindByIdAsync(user.Id);
                var roles = await _userManager.GetRolesAsync(foundUser);
                user.Roles = roles?.ToList();
            }

            return PagedList<UserDTO>.ToPagedList(userDTOs, userParameters.PageNumber, userParameters.PageSize);
        }

        /// <summary>
        /// Finds a user by its id
        /// </summary>
        /// <param name="id">Id of the user to find</param>
        /// <returns>Found user</returns>
        /// <exception cref="CardFileException"></exception>
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

        /// <summary>
        /// Sets receive notification status of user to given boolean value
        /// </summary>
        /// <param name="userId">Id of the user to change their receive notifications status</param>
        /// <param name="receiveNotifications">New value of receive notifications field of the user</param>
        /// <returns>User transfer object if data was valid</returns>
        /// <exception cref="CardFileException"></exception>
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

        /// <summary>
        /// Sends the list of text materials of the user to the given email
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="email">Email of the receiver</param>
        /// <returns>Task representing an asycnhronous operation</returns>
        public async Task SendListOfTextMaterialsAsPdf(string userId, string email)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new CardFileException("User id was not provided");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"User with id {userId} doesn't exist");
            }

            try
            {
                _emailService.SendListOfTextMaterialsOfTheUser(user, email);
            }
            catch (Exception e)
            {
                throw new CardFileException("Failed to send an email with pdf attached");
            }
        }
    }
}
