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
    /// <summary>
    /// Service to perform various operations regarding liked text materials such as getting all liked text materials of the user by user's id from the database,
    /// adding a text material to liked of the specified by id user, removing a text material from liked of the specified by id user
    /// </summary>
    public class LikedTextMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor which takes two arguments
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork interface</param>
        /// <param name="mapper">Instance of class that implements IMapper interface</param>
        public LikedTextMaterialService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Find the liked text material of the specified user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <returns>Liked text materials of the user</returns>
        /// <exception cref="CardFileException"></exception>
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

        /// <summary>
        /// Adds the text material to liked of the user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="textMaterialId">Text material id to add to liked</param>
        /// <returns>Tasl if data was valid</returns>
        /// <exception cref="CardFileException"></exception>
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

        /// <summary>
        /// Removes text material from liked of the user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="textMaterialId">Text material id to remove from liked</param>
        /// <returns>Task if data was valid</returns>
        /// <exception cref="CardFileException"></exception>
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
