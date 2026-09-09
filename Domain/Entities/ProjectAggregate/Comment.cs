using Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.ProjectAggregate
{
    public class Comment : BaseEntity
    {
        public string Content { get; private set; } = default!;
        public int TaskId { get; private set; }
        public Task Task { get; private set; } = default!;
        private Comment() { }
        internal Comment(string content, int taskId)
        {
            Content = content;
            TaskId = taskId;
        }
    }
}
