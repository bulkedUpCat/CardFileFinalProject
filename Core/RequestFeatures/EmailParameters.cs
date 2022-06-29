using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    /// <summary>
    /// Parameters that will be taken into account when generating a pdf file of a text material
    /// </summary>
    public class EmailParameters
    {
        public bool? Title { get; set; }
        public bool? Category { get; set; }
        public bool? Author { get; set; }
        public bool? DatePublished { get; set; }
    }
}
