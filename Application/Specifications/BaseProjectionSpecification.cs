using Application.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public class BaseProjectionSpecification<T, TResult> : BaseSpecification<T>, IProjectionSpecification<T, TResult>
    {
        public Expression<Func<T, TResult>> Selector { get; }
        protected BaseProjectionSpecification(Expression<Func<T, bool>> criteria, Expression<Func<T, TResult>> selector) : base(criteria)
        {
            Selector = selector;
        }

    }
}
