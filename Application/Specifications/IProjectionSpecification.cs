using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Application.Specifications
{
    public interface IProjectionSpecification<T, TResult> : ISpecification<T>
    {
        Expression<Func<T, TResult>> Selector { get;}
    }
}
