using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class TextMaterial
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public ApprovalStatus ApprovalStatus { get; set; } = ApprovalStatus.Pending;
        public int RejectCount { get; set; }

        public TextMaterialCategory TextMaterialCategory { get; set; }
        public int TextMaterialCategoryId { get; set; }

        public User Author { get; set; }
        public string AuthorId { get; set; }

        public DateTime DatePublished { get; set; } = DateTime.Now;
        public DateTime DateLastChanged { get; set; } = DateTime.Now;
        public DateTime DateApproved { get; set; }
    }
}
