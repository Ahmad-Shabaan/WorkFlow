using Application.Specifications;
using Task = Domain.Entities.Task;
namespace Application.Features.Comments.Specifications
{
    public class GetTaskByPublicIdWithComments : BaseSpecification<Task>
    {
        public GetTaskByPublicIdWithComments(Guid publicId) : base(p => p.PublicId == publicId)
        {
            Includes.Add(task => task.Comments);
        }
    }
}
