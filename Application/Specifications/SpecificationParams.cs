using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class SpecificationParams : ISpecificationParams
    {
        private const int MaxPageSize = 50;
        private int pageSize = 3;
        public int PageSize
        {
            get { return pageSize; }
            set { pageSize = value > 0 && value <= MaxPageSize ? value : MaxPageSize; }
        }


       
        public int PageIndex { get; set; } = 1;
        public string? Sort { get; set; } = null;

        private string? search;
        public string? Search
        {
            get { return search; }
            set { search = value?.ToLower(); }
        }


    }
}
