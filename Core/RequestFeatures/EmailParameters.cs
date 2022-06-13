using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    public class EmailParameters
    {
        public bool? Title { get; set; }
        public bool? Category { get; set; }
        public bool? Author { get; set; }
        public bool? DatePublished { get; set; }
    }
}
