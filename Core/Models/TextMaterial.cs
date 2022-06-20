using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public virtual TextMaterialCategory TextMaterialCategory { get; set; }
        public int TextMaterialCategoryId { get; set; }

        public virtual User Author { get; set; }
        [Column("AuthorId")]
        public string? AuthorId;

        public DateTime DatePublished { get; set; } = DateTime.Now;
        public DateTime DateLastChanged { get; set; } = DateTime.Now;
        public DateTime DateApproved { get; set; }

        public virtual ICollection<User> UsersWhoSaved { get; set; } = new List<User>();
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
