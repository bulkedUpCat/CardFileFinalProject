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
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(u => u.TextMaterials, src => src.MapFrom(x => x.TextMaterials));
        }
    }
}
