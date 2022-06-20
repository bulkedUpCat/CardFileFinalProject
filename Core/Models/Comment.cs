using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Comment
    {
        public int Id { get; set; }

        public Comment ParentComment { get; set; }
        public int? ParentCommentId { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public TextMaterial TextMaterial { get; set; }
        public int TextMaterialId { get; set; }

        public string Content { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
