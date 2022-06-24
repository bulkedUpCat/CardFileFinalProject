using AutoMapper;
using BLL.Validation;
using Core.DTOs;
using DAL.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class LikedTextMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LikedTextMaterialService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TextMaterialDTO>> GetLikedTextMaterialsByUserId(string userId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var likedTextMaterials = user.LikedTextMaterials;

            return _mapper.Map<IEnumerable<TextMaterialDTO>>(likedTextMaterials);
        }

        public async Task AddTextMaterialToLiked(string userId, int textMaterialId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialId}");
            }

            try
            {
                textMaterial.UsersWhoLiked.Add(user);
                
                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException("Failed to add a text material to liked");
            }
        }

        public async Task RemoveTextMaterialFromLiked(string userId, int textMaterialId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialId}");
            }

            if (!textMaterial.UsersWhoLiked.Contains(user))
            {
                throw new CardFileException($"User with id {userId} doesn't have a text material with id {textMaterialId} in his liked");
            }

            try
            {
                textMaterial.UsersWhoLiked.Remove(user);

                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }
    }
}
