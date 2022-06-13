using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class UpdateTextMaterialDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryTitle { get; set; }
        public int ApprovalStatusId { get; set; }
        public string AuthorId { get; set; }
    }
}
