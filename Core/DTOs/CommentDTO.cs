using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CommentDTO
    {
        public int Id { get; set; }
        public int? ParentCommentId { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
        public int TextMaterialId { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
