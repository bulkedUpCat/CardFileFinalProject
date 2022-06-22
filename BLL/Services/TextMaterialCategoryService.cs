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
    public class TextMaterialCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TextMaterialCategoryService(IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TextMaterialCategoryDTO>> GetTextMaterialCategoriesAsync()
        {
            var categories = await _unitOfWork.TextMaterialCategoryRepository.GetAsync();

            return _mapper.Map<IEnumerable<TextMaterialCategoryDTO>>(categories);
        }

        public async Task<TextMaterialCategoryDTO> GetTextMaterialCategoryById(int id)
        {
            var category = await _unitOfWork.TextMaterialCategoryRepository.GetByIdAsync(id);

            if (category == null)
            {
                throw new CardFileException($"Failed to find a category with id {id}");
            }

            return _mapper.Map<TextMaterialCategoryDTO>(category);
        }

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
    }
}
