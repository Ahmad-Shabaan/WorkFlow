using System.Linq.Expressions;
namespace Application.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public Expression<Func<T, bool>>? Criteria { get; }

        public List<Expression<Func<T, object>>> Includes { get; } = [];

        public List<string> IncludeStrings { get; } = [];

        // for sorting
        public Expression<Func<T, object>>? OrderBy { get; set; }
        public Expression<Func<T, object>>? OrderByDesc { get; set; }

        // for pagination
        public int Take { get; set; }
        public int Skip { get; set; }
        public bool IsPaginationEnabled { get; set; }

        public bool IsNotTrackable { get; set; } = false;
        public bool UseSplitQuery { get; set; } = false;

        protected BaseSpecification(Expression<Func<T, bool>> criteria)
        {
            Criteria = criteria;
        }
        public BaseSpecification()
        {

        }
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddInclude(string includeString)
        {
            IncludeStrings.Add(includeString);
        }

        protected void ApplyOrderByAsc(Expression<Func<T, object>> orderBy) // b => b.title
        {
            OrderBy = orderBy;
        }
        protected void ApplyOrderByDescending(Expression<Func<T, object>> orderByDesc)
        {
            OrderByDesc = orderByDesc;
        }
        public void ApplyPagination(int take, int skip)
        {
            IsPaginationEnabled = true;
            Take = take;
            Skip = skip;

        }
    }
}
