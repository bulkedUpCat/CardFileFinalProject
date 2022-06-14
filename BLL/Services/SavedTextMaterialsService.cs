using AutoMapper;
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
    public class SavedTextMaterialsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;

        public SavedTextMaterialsService(IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TextMaterialDTO>> GetSavedTextMaterialsOfUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var savedTextMaterials = user.SavedTextMaterials;

            return _mapper.Map<IEnumerable<TextMaterialDTO>>(savedTextMaterials);
        }

        public async Task AddTextMaterialToSaved(string userId, int textMaterialId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialId);

            if (textMaterialId == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialId}");
            }

            try
            {
                user.SavedTextMaterials.Add(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }
    }
}
