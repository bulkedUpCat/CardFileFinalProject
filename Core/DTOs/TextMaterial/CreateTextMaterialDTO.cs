using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreateTextMaterialDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryTitle { get; set; }
        public string AuthorId { get; set; }
    }
}
