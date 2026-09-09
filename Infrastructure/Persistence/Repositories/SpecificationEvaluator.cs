using Application.Specifications;
using Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class SpecificationEvaluator : ISpecificationEvaluator
    {

        private static IQueryable<T> ApplyBaseSpecifications<T>(ISpecification<T> spec , IQueryable<T> query) where T : class 
        {
            if (spec.Criteria != null)
            {
                query = query.Where(spec.Criteria);
            }
            // Apply includes (eager loading)
            query = spec.Includes.Aggregate(query, (acc, include) => acc.Include(include));
            query = spec.IncludeStrings.Aggregate(query, (acc, include) => acc.Include(include));

            // apply split query for multi collection included
            if (spec.UseSplitQuery)
                query = query.AsSplitQuery();

            //Apply sorting
            if (spec.OrderBy != null)
                query = query.OrderBy(spec.OrderBy);
            if (spec.OrderByDesc != null)
                query = query.OrderByDescending(spec.OrderByDesc);

            //Apply pagination
            if (spec.IsPaginationEnabled)
                query = query.Skip(spec.Skip).Take(spec.Take);

            if (spec.IsNotTrackable)
                query = query.AsNoTracking<T>();

            return query;

        }

        public IQueryable<T> GetQuery<T>(ISpecification<T> specification, IQueryable<T> query) where T : class
        {
            return ApplyBaseSpecifications(specification, query);
        }

        public IQueryable<TResult> GetQuery<T, TResult>(IProjectionSpecification<T, TResult> specification, IQueryable<T> query) where T : class
        {
            return ApplyBaseSpecifications(specification, query).Select(specification.Selector);
        }
    }
}
