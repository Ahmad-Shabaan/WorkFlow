using System.Linq.Expressions;
namespace Application.Specifications
{
    public class GetRecordsByCriteriaSpecification<T>(Expression<Func<T, bool>> criteria) : BaseSpecification<T>(criteria)
    {
    }
}

