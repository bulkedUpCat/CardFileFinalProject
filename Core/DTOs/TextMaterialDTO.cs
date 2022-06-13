using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class TextMaterialDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string CategoryTitle { get; set; }
        public int ApprovalStatusId { get; set; }
        public int RejectCount { get; set; }
        public string AuthorId { get; set; }
        public string UserName { get; set; }
        public DateTime DatePublished { get; set; }
    }
}
