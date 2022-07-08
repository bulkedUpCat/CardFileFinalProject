using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class UpdateBanDTO
    {
        public string Reason { get; set; }
        public int Days { get; set; }
        public string UserId { get; set; }
    }
}
