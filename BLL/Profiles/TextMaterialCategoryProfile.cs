using AutoMapper;
using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Profiles
{
    /// <summary>
    /// AutoMapper profile for TextMaterialCategory entity
    /// </summary>
    public class TextMaterialCategoryProfile : Profile
    {
        public TextMaterialCategoryProfile()
        {
            CreateMap<TextMaterialCategory, TextMaterialCategoryDTO>()
                .ReverseMap();

            CreateMap<CreateTextMaterialCategoryDTO, TextMaterialCategory>();
        }
    }
}
