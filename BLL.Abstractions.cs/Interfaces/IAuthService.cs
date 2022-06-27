using Core.DTOs;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Abstractions.cs.Interfaces
{
    /// <summary>
    /// Interface to be implemented by class which perform various authentication and authorization operations
    /// </summary>
    public interface IAuthService
    {
        Task<User> LogInAsync(UserLoginDTO user);
        Task<User> SignUpAsync(UserRegisterDTO user);
    }
}
