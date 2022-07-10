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
    /// Automapper profile for Ban entity
    /// </summary>
    public class BanProfile: Profile
    {
        public BanProfile()
        {
            CreateMap<Ban, BanDTO>();
            CreateMap<CreateBanDTO, Ban>();
        }
    }
}
