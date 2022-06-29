using AutoMapper;
using BLL.Abstractions.cs.Interfaces;
using BLL.Validation;
using Core.DTOs;
using Core.RequestFeatures;
using DAL.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services
{
    /// <summary>
    /// Service to perform various operations regarding saving text materials such as getting saved text materials of the user by their id,
    /// adding a text material to saved of the specified by id user, removing a text material from saved of the specified by id user
    /// </summary>
    public class SavedTextMaterialService: ISavedTextMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor which takes two arguments
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork inteface</param>
        /// <param name="mapper">Instance of class that implements IMapper interface</param>
        public SavedTextMaterialService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Find saved text materials of the specified user by given parameters
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="textMaterialParams">Text material parameters to take into account</param>
        /// <returns>Saved text materials of the user which satisfy the parameters</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<PagedList<TextMaterialDTO>> GetSavedTextMaterialsOfUser(string userId, TextMaterialParameters textMaterialParams)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"User with id {userId} doens't exist");
            }

            var textMaterials = await _unitOfWork.TextMaterialRepository.GetWithDetailsAsync(textMaterialParams);
            var savedTextMaterials = textMaterials.Where(tm => user.SavedTextMaterials.Any(x => x.Id == tm.Id));

            return PagedList<TextMaterialDTO>
                .ToPagedList(_mapper.Map<IEnumerable<TextMaterialDTO>>(savedTextMaterials), textMaterialParams.PageNumber, textMaterialParams.PageSize);
        }

        /// <summary>
        /// Adds text material to saved of the specified user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="textMaterialId">Id of text material to add to saved</param>
        /// <returns>Task if data was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task AddTextMaterialToSaved(string userId, int textMaterialId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialId}");
            }

            try
            {
                textMaterial.UsersWhoSaved.Add(user);

                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        /// <summary>
        /// Removes text material from saved of the user
        /// </summary>
        /// <param name="userId">Id of the user</param>
        /// <param name="textMaterialId">Id of text material to remove from saved</param>
        /// <returns>Task if data was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task RemoveTextMaterialFromSaved(string userId, int textMaterialId)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialId}");
            }

            if (!textMaterial.UsersWhoSaved.Any(u => u.Id == user.Id))
            {
                throw new CardFileException($"User with id {userId} doesn't have a text material with id {textMaterialId} in his saved");
            }

            try
            {
                textMaterial.UsersWhoSaved.Remove(user);

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
