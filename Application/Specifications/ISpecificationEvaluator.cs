using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications
{
    public interface ISpecificationEvaluator
    {
        IQueryable<T> GetQuery<T>(ISpecification<T> specification, IQueryable<T> query) where T : class;

        IQueryable<TResult> GetQuery<T, TResult>(IProjectionSpecification<T, TResult> specification, IQueryable<T> query) where T : class;
    }
}
