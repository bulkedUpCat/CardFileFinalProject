using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.RequestFeatures
{
    public class TextMaterialParameters : RequestParameters
    {
        public TextMaterialParameters()
        {
            OrderBy = "datePublished";
        }

        public string? StartDate { get; set; }
        public string? EndDate { get; set; }
        public string? SearchTitle { get; set; }
        public string? SearchCategory { get; set; }
        public string? SearchAuthor { get; set; }
        public string? UserId { get; set; }
        public List<int>? ApprovalStatus { get; set; }

        public bool ValidDateRange()
        {
            return DateTime.Compare(DateTime.Parse(StartDate), DateTime.Parse(EndDate)) <= 0;
        }
    }
}
