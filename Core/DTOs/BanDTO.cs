﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class BanDTO
    {
        public int Id { get; set; }
        public string Reason { get; set; }
        public DateTime Expires { get; set; }
        public string UserId { get; set; }
    }
}
