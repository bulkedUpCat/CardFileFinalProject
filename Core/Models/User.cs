using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User : IdentityUser
    {
        public bool ReceiveNotifications { get; set; }
        public virtual ICollection<TextMaterial> TextMaterials { get; set; }
        public virtual ICollection<TextMaterial> SavedTextMaterials { get; set; } = new List<TextMaterial>();
    }
}
