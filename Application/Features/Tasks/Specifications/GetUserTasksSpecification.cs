using Application.Specifications;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Tasks.Specifications
{
    public class GetUserTasksSpecification : BaseSpecification<TaskAssignment>
    {
        public GetUserTasksSpecification(int userId, List<int> tasksIdx) : base(x => x.UserId == userId && tasksIdx.Contains(x.TaskId))
        {
            AddInclude(x => x.Task);
        }
    }
}
