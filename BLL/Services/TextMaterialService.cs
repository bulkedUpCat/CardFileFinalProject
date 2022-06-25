using AutoMapper;
using BLL.Abstractions.cs.Interfaces;
using BLL.Validation;
using Core.DTOs;
using Core.Models;
using Core.RequestFeatures;
using DAL.Abstractions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class TextMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public TextMaterialService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
           
            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {id}");
            }

            var textMaterials = await _unitOfWork.TextMaterialRepository.GetByUserId(user.Id, textMaterialParams);

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

            return _mapper.Map<TextMaterialDTO>(textMaterial);
        }

        public async Task<TextMaterialDTO> CreateTextMaterial(CreateTextMaterialDTO textMaterialDTO)
        {
            var textMaterial = _mapper.Map<TextMaterial>(textMaterialDTO);

            var category = await _unitOfWork.TextMaterialCategoryRepository.GetByTitleAsync(textMaterialDTO.CategoryTitle);

            if (category == null)
            {
                throw new CardFileException($"Failed to find a category with title {textMaterialDTO.CategoryTitle}");
            }

            var author = await _unitOfWork.UserRepository.GetByIdAsync(textMaterialDTO.AuthorId);

            if (author == null)
            {
                throw new CardFileException($"Failed to find a user with id {textMaterialDTO.AuthorId}");
            }

            textMaterial.TextMaterialCategory = category;
            textMaterial.Author = author;

            try
            {
                await _unitOfWork.TextMaterialRepository.CreateAsync(textMaterial);
                await _unitOfWork.SaveChangesAsync();
                
                if (author.ReceiveNotifications)
                {
                    _emailService.NotifyThatTextMaterialWasCreated(author, textMaterial);
                }    
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
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(id);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {id}");
            }

            try
            {
                await _unitOfWork.TextMaterialRepository.DeleteById(id);
                await _unitOfWork.SaveChangesAsync();

                if (textMaterial.Author.ReceiveNotifications)
                {
                    _emailService.NotifyThatTextMaterialWasDeleted(textMaterial.Author, textMaterial);
                }
            }
            catch (Exception ex)
            {
                throw new CardFileException(ex.Message);
            }
        }

        public async Task ApproveTextMaterial(int textMaterialId)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Text material with id {textMaterialId} doesn't exist");
            }

            if (textMaterial.ApprovalStatus == ApprovalStatus.Approved)
            {
                throw new CardFileException($"Text material with id {textMaterialId} is already approved");
            }

            if (textMaterial.ApprovalStatus == ApprovalStatus.Rejected)
            {
                throw new CardFileException($"Text material with id {textMaterialId} is already rejected");
            }

            textMaterial.ApprovalStatus = ApprovalStatus.Approved;

            try
            {
                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();
                
                if (textMaterial.Author.ReceiveNotifications)
                {
                    _emailService.NotifyThatTextMaterialWasApproved(textMaterial.Author, textMaterial);
                }
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }

        public async Task RejectTextMaterial(int textMaterialId, string? rejectMessage = null)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Text material with id {textMaterialId} doesn't exist");
            }

            if (textMaterial.ApprovalStatus == ApprovalStatus.Rejected)
            {
                throw new CardFileException($"Text material with id {textMaterialId} is already rejected");
            }

            if (textMaterial.ApprovalStatus == ApprovalStatus.Approved)
            {
                throw new CardFileException($"Text material with id {textMaterialId} is already approved");
            }

            textMaterial.ApprovalStatus= ApprovalStatus.Rejected;
            textMaterial.RejectCount++;
            textMaterial.RejectMessage = rejectMessage;

            try
            {
                _unitOfWork.TextMaterialRepository.Update(textMaterial);
                await _unitOfWork.SaveChangesAsync();

                if (textMaterial.Author.ReceiveNotifications)
                {
                    _emailService.NotifyThatTextMaterialWasRejected(textMaterial.Author, textMaterial, rejectMessage);
                }
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

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);

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
                _emailService.SendTextMaterialAsPdf(user, textMaterial, emailParams);
            }
            catch (Exception e)
            {
                throw new CardFileException("Failed to send an email with pdf attached");
            }
        }
    }
}
