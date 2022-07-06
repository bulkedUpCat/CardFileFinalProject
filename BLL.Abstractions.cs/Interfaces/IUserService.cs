using Core.DTOs;
using Core.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to be implemented by classes to work with users
    /// </summary>
    public interface IUserService
    {
        Task<PagedList<UserDTO>> GetAll(UserParameters userParameters);
        Task<UserDTO> GetUserById(string id);
        Task<UserDTO> ToggleReceiveNotifications(string userId, bool receiveNotifications);
        Task SendListOfTextMaterialsAsPdf(string userId, string email);
    }
}
