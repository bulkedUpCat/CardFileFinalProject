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
    /// <summary>
    /// Service to perform various operations on TextMaterial entities such as getting all text materials from the database,
    /// getting text materials of the particular author by the author's id, getting the text material from the database by its id,
    /// adding a text material to database, updating a text material in database, removing a text material from database, 
    /// changing the approval status of the text material, sending a text material and optionally its info on email of the specified by id user as a pdf file
    /// </summary>
    public class TextMaterialService: ITextMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Constructor that accepts to unitOfWork to access the repositories, mapper to map entities and emailService to send emails
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork interface</param>
        /// <param name="mapper">Instance of class that implements IMapper interface</param>
        /// <param name="emailService">Instance of class that implements IEmailService interface</param>
        public TextMaterialService(IUnitOfWork unitOfWork, 
            IMapper mapper,
            IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailService = emailService;
        }

        /// <summary>
        /// Finds all text materials in database that satisfy given parameters
        /// </summary>
        /// <param name="parameters">Parameters to take into account</param>
        /// <returns>All text materials that satisfy the given parameters</returns>
        public async Task<PagedList<TextMaterialDTO>> GetTextMaterials(TextMaterialParameters parameters)
        {
            var textMaterials = await _unitOfWork.TextMaterialRepository.GetWithDetailsAsync(parameters);

            if (textMaterials.Count() == 0)
            {
                throw new NotFoundException("No text materials were found");
            }

            return PagedList<TextMaterialDTO>
                .ToPagedList(_mapper.Map<IEnumerable<TextMaterialDTO>>(textMaterials),parameters.PageNumber,parameters.PageSize);
        }
        
        /// <summary>
        /// Finds the text materials created by the given user by their id
        /// </summary>
        /// <param name="id">Id of the author of the textMaterial</param>
        /// <param name="textMaterialParams">Parameters to take into account</param>
        /// <returns>All text materials that were created by the author by their id</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<PagedList<TextMaterialDTO>> GetTextMaterialsOfUser(string id, TextMaterialParameters textMaterialParams)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);
           
            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {id}");
            }

            var textMaterials = await _unitOfWork.TextMaterialRepository.GetByUserId(user.Id, textMaterialParams);

            if (textMaterials.Count() == 0)
            {
                throw new NotFoundException($"No text materials of author with id {user.Id} were found");
            }

            return PagedList<TextMaterialDTO>
                .ToPagedList(_mapper.Map<IEnumerable<TextMaterialDTO>>(textMaterials), textMaterialParams.PageNumber, textMaterialParams.PageSize);
        }

        /// <summary>
        /// Finds a text material by its id
        /// </summary>
        /// <param name="id">Id of the text material to find</param>
        /// <returns>Found text material if id was valid</returns>
        public async Task<TextMaterialDTO> GetTextMaterialById(int id)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(id);

            if (textMaterial == null)
            {
                throw new NotFoundException($"Text material with id {id} doesn't exist");
            }

            return _mapper.Map<TextMaterialDTO>(textMaterial);
        }

        /// <summary>
        /// Adds a text material to database and optionally notifies the user
        /// </summary>
        /// <param name="textMaterialDTO">Text material data to create</param>
        /// <returns>Newly created text material transfer object</returns>
        /// <exception cref="CardFileException"></exception>
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

        /// <summary>
        /// Apdates a text material in database
        /// </summary>
        /// <param name="textMaterialDTO">text material data to update</param>
        /// <returns>Task if model was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task UpdateTextMaterial(UpdateTextMaterialDTO textMaterialDTO)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialDTO.Id);

            if (textMaterial == null)
            {
                throw new NotFoundException($"Failed to find a text material with id {textMaterialDTO.Id}");
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

        /// <summary>
        /// Removes a text material from database by its id and optionally notifies the user
        /// </summary>
        /// <param name="id">Id of the text material to delete</param>
        /// <returns>Task if id was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task DeleteTextMaterial(int id)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdWithDetailsAsync(id);

            if (textMaterial == null)
            {
                throw new NotFoundException($"Failed to find a text material with id {id}");
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

        /// <summary>
        /// Approves a text material by its id and optionally notifies the user
        /// </summary>
        /// <param name="textMaterialId">Id of the text material to approve</param>
        /// <returns>Task if id was valid</returns>
        /// <exception cref="CardFileException"></exception>
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

        /// <summary>
        /// Rejects text material by its id and sends the author on their email the reject message
        /// </summary>
        /// <param name="textMaterialId">Id of text material to reject</param>
        /// <param name="rejectMessage">Reason why text material was rejected</param>
        /// <returns>Task if provided data was valid</returns>
        /// <exception cref="CardFileException"></exception>
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

        /// <summary>
        /// Sends text material data to given user's email
        /// </summary>
        /// <param name="userId">Id of the user to receive data about the text material</param>
        /// <param name="textMaterialId">Id of the text material which data is to be sent on user's email</param>
        /// <param name="emailParams">Parameters to take into accout when generating a pdf file</param>
        /// <returns>Task if provided data was valid</returns>
        /// <exception cref="CardFileException"></exception>
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
