using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class UserRegisterDTO
    {
        public string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
