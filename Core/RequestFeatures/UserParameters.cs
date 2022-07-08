using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    /// <summary>
    /// Parameters for users to perform paging, sorting
    /// </summary>
    public class UserParameters : RequestParameters
    {
        /// <summary>
        /// Constructor that sets order by to default empty string
        /// </summary>
        public UserParameters()
        {
            OrderBy = String.Empty;
        }

        public string? UserName { get; set; }
        public string? Email { get; set; }
        public bool IsBanned { get; set; }
    }
}
