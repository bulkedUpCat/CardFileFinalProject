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
    public class TextMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly EmailService _emailService;

        public TextMaterialService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            UserManager<User> userManager,
            EmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<PagedList<TextMaterialDTO>> GetTextMaterials(TextMaterialParameters parameters)
        {
            var textMaterials = await _unitOfWork.TextMaterialRepository.GetWithDetailsAsync(parameters);

            return PagedList<TextMaterialDTO>
                .ToPagedList(_mapper.Map<IEnumerable<TextMaterialDTO>>(textMaterials),parameters.PageNumber,parameters.PageSize);
        }
        
        public async Task<PagedList<TextMaterialDTO>> GetTextMaterialsOfUser(string id, TextMaterialParameters textMaterialParams)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {id}");
            }

            var textMaterials = await _unitOfWork.TextMaterialRepository.GetByUser(user, textMaterialParams);

            if (textMaterials == null)
            {
                throw new CardFileException($"No text materials of author with id {user.Id} were found");
            }

            return PagedList<TextMaterialDTO>
                .ToPagedList(_mapper.Map<IEnumerable<TextMaterialDTO>>(textMaterials), textMaterialParams.PageNumber, textMaterialParams.PageSize);
        }

        public async Task<TextMaterialDTO> GetTextMaterialById(int id)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(id);

            if (textMaterial == null)
            {
                throw new CardFileException($"Text material with id {id} doesn't exist");
            }

            return _mapper.Map<TextMaterialDTO>(textMaterial);
        }

        public async Task<TextMaterialDTO> CreateTextMaterial(CreateTextMaterialDTO textMaterialDTO)
        {
            var textMaterial = _mapper.Map<TextMaterial>(textMaterialDTO);

            var category = await _unitOfWork.TextMaterialCategoryRepository.GetByTitleAsync(textMaterialDTO.CategoryTitle);
            var author = await _userManager.FindByIdAsync(textMaterialDTO.AuthorId);

            textMaterial.TextMaterialCategory = category;
            textMaterial.Author = author;

            try
            {
                await _unitOfWork.TextMaterialRepository.CreateAsync(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardFileException(ex.Message);
            }

            return _mapper.Map<TextMaterialDTO>(textMaterial);
        }

        public async Task UpdateTextMaterial(UpdateTextMaterialDTO textMaterialDTO)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialDTO.Id);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialDTO.Id}");
            }

            try
            {
                var category = await _unitOfWork.TextMaterialCategoryRepository.GetByTitleAsync(textMaterialDTO.CategoryTitle);

                if (category != null &&
                    category.Title != textMaterialDTO.CategoryTitle)
                {
                    textMaterial.TextMaterialCategory = category;
                }

                textMaterial.Title = textMaterialDTO.Title;
                textMaterial.Content = textMaterialDTO.Content;
                textMaterial.ApprovalStatus = ApprovalStatus.Pending;
                textMaterial.DateLastChanged = DateTime.Now;

                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardFileException(ex.Message);
            }
        }

        public async Task DeleteTextMaterial(int id)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(id);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {id}");
            }

            try
            {
                _unitOfWork.TextMaterialRepository.DeleteEntity(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CardFileException(ex.Message);
            }
        }

        public async Task ApproveTextMaterial(int textMaterialId)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Text material with id {textMaterialId} doesn't exist");
            }

            if (textMaterial.ApprovalStatus == ApprovalStatus.Approved)
            {
                throw new CardFileException($"Text material with id {textMaterialId} is already approved");
            }

            textMaterial.ApprovalStatus = ApprovalStatus.Approved;

            try
            {
                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        public async Task RejectTextMaterial(int textMaterialId)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Text material with id {textMaterialId} doesn't exist");
            }

            if (textMaterial.ApprovalStatus == ApprovalStatus.Rejected)
            {
                throw new CardFileException($"Text material with id {textMaterialId} is already rejected");
            }

            textMaterial.ApprovalStatus= ApprovalStatus.Rejected;
            textMaterial.RejectCount++;

            try
            {
                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        public async Task SendTextMaterialAsPdf(string userId, int textMaterialId, EmailParameters emailParams)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new CardFileException("User id was not provided");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"User with id {userId} doesn't exist");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Text material with id {textMaterialId} doesn't exist");
            }

            try
            {
                await _emailService.SendTextMaterialAsPdf(user, textMaterial, emailParams);
            }
            catch (Exception e)
            {
                throw new CardFileException("Failed to send an email with pdf attached");
            }
        }

        public async Task<IEnumerable<TextMaterialDTO>> GetSavedTextMaterialsOfUser(string userId)
        {
            var savedTextMaterials = await _unitOfWork.UserRepository.GetSavedTextMaterialsByUserId(userId);
            return _mapper.Map <IEnumerable<TextMaterialDTO>>(savedTextMaterials);
        }

        public async Task AddTextMaterialToSaved(string userId, int textMaterialId)
        {
            var user = await _userManager.FindByIdAsync(userId);

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
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {userId}");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialId}");
            }

            if (!textMaterial.UsersWhoSaved.Contains(user))
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
