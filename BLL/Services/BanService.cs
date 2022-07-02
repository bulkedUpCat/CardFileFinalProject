using AutoMapper;
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
    public class BanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BanService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BanDTO>> GetAllBans()
        {
            var bans =  await _unitOfWork.BanRepository.GetAsync();
            return _mapper.Map<IEnumerable<BanDTO>>(bans);
        }

        public async Task<BanDTO> GetBanById(int id)
        {
            var ban = await _unitOfWork.BanRepository.GetByIdAsync(id);
            return _mapper.Map<BanDTO>(ban);
        }

        public async Task<BanDTO> GetBanByUserId(string userId)
        {
            var ban = await _unitOfWork.BanRepository.GetByUserIdAsync(userId);
            return _mapper.Map<BanDTO>(ban);
        }

        public async Task<BanDTO> BanUser(CreateBanDTO banDTO)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(banDTO.UserId);

            if (user == null)
            {
                throw new CardFileException("Failed to find a user");
            }

            var userIsAlreadyBanned = false;

            var existingBan = await _unitOfWork.BanRepository.GetByUserIdAsync(banDTO.UserId);

            if (existingBan != null)
            {
                if (existingBan.Expires > DateTime.Now)
                {
                    existingBan.Expires = existingBan.Expires.AddDays(banDTO.Days);
                }
                else
                {
                    existingBan.Expires = DateTime.Now.AddDays(banDTO.Days);
                }

                existingBan.Reason += $"\n{banDTO.Reason}";
                userIsAlreadyBanned = true;
            }

            var ban = _mapper.Map<Ban>(banDTO);
            ban.Expires = DateTime.Now.AddDays(banDTO.Days);

            try
            {
                if (userIsAlreadyBanned)
                {
                    _unitOfWork.BanRepository.Update(existingBan);
                }
                else
                {
                    await _unitOfWork.BanRepository.CreateAsync(ban);
                }

                await _unitOfWork.SaveChangesAsync();

                return userIsAlreadyBanned ? _mapper.Map<BanDTO>(existingBan) : _mapper.Map<BanDTO>(ban);
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

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
