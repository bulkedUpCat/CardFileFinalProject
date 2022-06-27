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
    /// AutoMapper profile for Comment entity
    /// </summary>
    public class CommentProfile: Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDTO>()
                .ForMember(c => c.UserName, src => src.MapFrom(x => x.User.UserName))
                .ForMember(c => c.UserId, src => src.MapFrom(x => x.User.Id));

            CreateMap<CreateCommentDTO, Comment>();
        }
    }
}
