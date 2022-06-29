using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    /// <summary>
    /// Parameters for users. To perform paging, sorting
    /// </summary>
    public class UserParameters : RequestParameters
    {
        public UserParameters()
        {
            OrderBy = String.Empty;
        }
    }
}
