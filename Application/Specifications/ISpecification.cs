using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public interface ISpecification<T>
    {
        Expression<Func<T, bool>>? Criteria { get; }
        List<Expression<Func<T, object>>> Includes { get; }
        List<string> IncludeStrings { get; }
        // for sorting
        public Expression<Func<T, object>>? OrderBy { get; set; } // b => b.title
        public Expression<Func<T, object>>? OrderByDesc { get; set; }
        // for pagination
        int Skip { get; set; }
        int Take { get; set; }
        bool IsPaginationEnabled { get; set; }
        bool IsNotTrackable { get; set; }
        bool UseSplitQuery { get; set; }
    }
}
