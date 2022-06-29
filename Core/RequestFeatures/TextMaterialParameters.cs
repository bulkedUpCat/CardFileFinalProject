using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    /// <summary>
    /// Parameters that will be taken into account when returning text materials: start, end date of publishment, 
    /// search strings for title, category, author, orderby string and array of approval status
    /// </summary>
    public class TextMaterialParameters : RequestParameters
    {
        /// <summary>
        /// Constructor that sets orderby to datePublished desc by default, meaning text material will be automatically
        /// sorted by publishment date descending
        /// </summary>
        public TextMaterialParameters()
        {
            OrderBy = "datePublished desc";
        }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? SearchTitle { get; set; }
        public string? SearchCategory { get; set; }
        public string? SearchAuthor { get; set; }
        public List<int>? ApprovalStatus { get; set; }
    }
}
