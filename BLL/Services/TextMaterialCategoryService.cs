using AutoMapper;
using BLL.Abstractions.cs.Interfaces;
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
    /// Service to perform various operations on TextMaterialCategory entities such as getting all categories from the database,
    /// getting a category by its id, adding a text material category to database, removing a text material category from the database
    /// </summary>
    public class TextMaterialCategoryService: ITextMaterialCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        /// <summary>
        /// Constructor which takes two arguments
        /// </summary>
        /// <param name="unitOfWork">Instance of class that implements IUnitOfWork interface</param>
        /// <param name="mapper">Instance of class that implements IMapper interface</param>
        public TextMaterialCategoryService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        /// <summary>
        /// Finds all existing text material categories in database
        /// </summary>
        /// <returns>All existing text material categories</returns>
        public async Task<IEnumerable<TextMaterialCategoryDTO>> GetTextMaterialCategoriesAsync()
        {
            var categories = await _unitOfWork.TextMaterialCategoryRepository.GetAsync();

            return _mapper.Map<IEnumerable<TextMaterialCategoryDTO>>(categories);
        }

        /// <summary>
        /// Finds a text material category by its id
        /// </summary>
        /// <param name="id">Id of text material category to find</param>
        /// <returns>Correct text material category</returns>
        public async Task<TextMaterialCategoryDTO> GetTextMaterialCategoryById(int id)
        {
            var category = await _unitOfWork.TextMaterialCategoryRepository.GetByIdAsync(id);

            return _mapper.Map<TextMaterialCategoryDTO>(category);
        }

        /// <summary>
        /// Adds new text material category to database
        /// </summary>
        /// <param name="categoryDTO">category Transfer object to create</param>
        /// <returns>Newly created text material category transfer object</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task<TextMaterialCategoryDTO> CreateTextMaterialCategoryAsync(CreateTextMaterialCategoryDTO categoryDTO)
        {
            var existingCategory = await _unitOfWork.TextMaterialCategoryRepository.GetByTitleAsync(categoryDTO.Title);

            if (existingCategory != null)
            {
                throw new CardFileException($"Text material category with title {categoryDTO.Title} already exists");
            }

            var category = _mapper.Map<TextMaterialCategory>(categoryDTO);

            try
            {
                await _unitOfWork.TextMaterialCategoryRepository.CreateAsync(category);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
            

            return _mapper.Map<TextMaterialCategoryDTO>(category);
        }

        /// <summary>
        /// Removes text material category from database by its id
        /// </summary>
        /// <param name="id">Id of text material category to delete</param>
        /// <returns>Task if id was valid</returns>
        /// <exception cref="CardFileException"></exception>
        public async Task DeleteTextMaterialCategoryAsync(int id)
        {
            var category = await _unitOfWork.TextMaterialCategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new CardFileException($"Text material category with id {id} doesn't exist");
            }

            try
            {
                _unitOfWork.TextMaterialCategoryRepository.Delete(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new CardFileException(e.Message);
            }
        }
    }
}
