using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public interface ISpecificationParams
    {
        public string? Sort { get; set; }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
    }
}
