using AutoMapper;
using BLL.Abstractions.cs.Interfaces;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using DAL.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Service to perform various operations regarding Ban entities such as getting all bans from database, getting ban by its or user's id,
    /// banning and unbanning users
    /// </summary>
    public class BanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor that accepts unitOfWork to access repositories, mapper to map entities, emailService to send notifications on email
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork interface</param>
        /// <param name="mapper">Instance of class that implements IMapper interface</param>
        /// <param name="emailService">Instance of class that implements IEamilService interface</param>
        public BanService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService; ;
        }

        /// <summary>
        /// Finds all existing bans
        /// </summary>
        /// <returns>List of all bans from the database</returns>
        public async Task<IEnumerable<BanDTO>> GetAllBans()
        {
            var bans =  await _unitOfWork.BanRepository.GetAsync();
            return _mapper.Map<IEnumerable<BanDTO>>(bans);
        }

        /// <summary>
        /// Finds a single ban with given id
        /// </summary>
        /// <param name="id">Id of the ban to return</param>
        /// <returns>Single ban with specified id</returns>
        public async Task<BanDTO> GetBanById(int id)
        {
            var ban = await _unitOfWork.BanRepository.GetByIdAsync(id);
            return _mapper.Map<BanDTO>(ban);
        }

        /// <summary>
        /// Finds a single ban by user's id
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Single ban with specified user id</returns>
        public async Task<BanDTO> GetBanByUserId(string userId)
        {
            var ban = await _unitOfWork.BanRepository.GetByUserIdAsync(userId);
            return _mapper.Map<BanDTO>(ban);
        }

        /// <summary>
        /// Adds new ban to the database
        /// </summary>
        /// <param name="banDTO">Model to add to database</param>
        /// <returns>Data transfer object of the newly created ban</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<BanDTO> BanUser(CreateBanDTO banDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(banDTO.UserId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {banDTO.UserId}");
            }

            var existingBan = await _unitOfWork.BanRepository.GetByUserIdAsync(banDTO.UserId);

            if (existingBan != null)
            {
                throw new CardFileException($"User with id {banDTO.UserId} is already banned");
            }

            var ban = _mapper.Map<Ban>(banDTO);
            ban.Expires = DateTime.Now.AddDays(banDTO.Days);

            try
            {
                await _unitOfWork.BanRepository.CreateAsync(ban);
                await _unitOfWork.SaveChangesAsync();

                if (user.ReceiveNotifications)
                {
                    _emailService.NotifyThatUserWasBanned(user, ban);
                }

                return _mapper.Map<BanDTO>(ban);
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Updates existing ban in the database
        /// </summary>
        /// <param name="banDTO">Model that contains information of the ban to update</param>
        /// <returns>Data transfer object of an updated ban</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<BanDTO> UpdateExistingBan(UpdateBanDTO banDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(banDTO.UserId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {banDTO.UserId}");
            }

            var ban = await _unitOfWork.BanRepository.GetByUserIdAsync(banDTO.UserId);

            if (ban == null)
            {
                throw new CardFileException($"Failed to find a ban with user id {banDTO.UserId}");
            }

            if (ban.Expires > DateTime.Now)
            {
                ban.Expires = ban.Expires.AddDays(banDTO.Days);
            }
            else
            {
                ban.Expires = DateTime.Now.AddDays(banDTO.Days);
            }

            ban.Reason += $"\n{banDTO.Reason}";

            try
            {
                _unitOfWork.BanRepository.Update(ban);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<BanDTO>(ban);
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Removes ban from the database by its id
        /// </summary>
        /// <param name="id">Id of the ban to remove from the database</param>
        /// <returns>Task representing an asynchronous operation</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task DeleteBanById(int id)
        {
            var ban = await _unitOfWork.BanRepository.GetByIdAsync(id);

            if (ban == null)
            {
                throw new CardFileException($"Failed to find a ban with id {id}");
            }

            try
            {
                await _unitOfWork.BanRepository.DeleteById(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Removes ban from the database by its user's id
        /// </summary>
        /// <param name="id">Id of the user</param>
        /// <returns>Task representing an asynchronous operation</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task DeleteBanByUserId(string id)
        {
            var ban = await _unitOfWork.BanRepository.GetByUserIdAsync(id);

            if (ban == null)
            {
                throw new CardFileException($"Failed to find a ban with user id {id}");
            }

            try
            {
                await _unitOfWork.BanRepository.DeleteByUserId(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }
    }
}
