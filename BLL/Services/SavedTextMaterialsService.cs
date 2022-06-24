using AutoMapper;
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
    public class SavedTextMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SavedTextMaterialsService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

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
