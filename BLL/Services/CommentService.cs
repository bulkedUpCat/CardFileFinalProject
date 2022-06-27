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
    /// <summary>
    /// Service to perform various operations regarding Comment entities such as getting all comment of the particular text material from the database,
    /// adding a comment to database, updating a comment in database, deleting a comment from database
    /// </summary>
    public class CommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor that takes two arguments
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork interface</param>
        /// <param name="mapper">Instance of class that implemetns IMapper interface</param>
        public CommentService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Finds the comments by the text material id
        /// </summary>
        /// <param name="textMaterialId">Id of the text material</param>
        /// <returns>Correct comments if textMaterialId was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<IEnumerable<CommentDTO>> GetCommentsOfTextMaterial(int textMaterialId)
        {
            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(textMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {textMaterialId}");
            }

            var comments = await _unitOfWork.CommentRepository.GetCommentsOfTextMaterial(textMaterialId);

            return _mapper.Map<IEnumerable<CommentDTO>>(comments);
        }

        /// <summary>
        /// Adds a comment to the database
        /// </summary>
        /// <param name="createCommentDTO">Comment model to add</param>
        /// <returns>Comment transfer object if model was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<CommentDTO> CreateComment(CreateCommentDTO createCommentDTO)
        {
            if (createCommentDTO == null)
            {
                throw new CardFileException("Model is invalid");
            }

            if (string.IsNullOrWhiteSpace(createCommentDTO.Content))
            {
                throw new CardFileException("Content of a comment must not be empty");
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync(createCommentDTO.UserId);

            if (user == null)
            {
                throw new CardFileException($"Failed to find a user with id {createCommentDTO.UserId}");
            }

            var textMaterial = await _unitOfWork.TextMaterialRepository.GetByIdAsync(createCommentDTO.TextMaterialId);

            if (textMaterial == null)
            {
                throw new CardFileException($"Failed to find a text material with id {createCommentDTO.TextMaterialId}");
            }

            var parentComment = await _unitOfWork.CommentRepository.GetCommentById(createCommentDTO?.ParentCommentId);

            var comment = _mapper.Map<Comment>(createCommentDTO);
            comment.User = user;
            comment.TextMaterial = textMaterial;
            comment.ParentComment = parentComment;

            try
            {
                await _unitOfWork.CommentRepository.CreateAsync(comment);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (CardFileException e)
            {
                throw new CardFileException(e.Message);
            }

            return _mapper.Map<CommentDTO>(comment);
        }

        /// <summary>
        /// Updates comment in database
        /// </summary>
        /// <param name="commentDTO">Comment model to update</param>
        /// <returns>Comment transfer object if model was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<CommentDTO> UpdateComment(UpdateCommentDTO commentDTO)
        {
            var comment = await _unitOfWork.CommentRepository.GetCommentById(commentDTO.Id);

            if (comment == null)
            {
                throw new CardFileException($"Failed to find a comment with id {commentDTO.Id}");
            }

            comment.Content = commentDTO.Content;

            try
            {
                _unitOfWork.CommentRepository.Update(comment);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException("Failed to update a comment");
            }

            return _mapper.Map<CommentDTO>(comment);
        }

        /// <summary>
        /// Deketes comment from database
        /// </summary>
        /// <param name="id">Id of the comment to delete</param>
        /// <returns>Task if the comment was found and deleted</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task DeleteComment(int id)
        {
            var comment = await _unitOfWork.CommentRepository.GetCommentById(id);

            if (comment == null)
            {
                throw new CardFileException($"Failed to find a comment with id {id}");
            }

            try
            {
                await _unitOfWork.CommentRepository.Delete(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException("Failed to delete a comment");
            }
        }
    }
}
