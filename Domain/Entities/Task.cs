using Domain.Common;
using Domain.Exceptions;
using TaskStatus = Domain.Enums.TaskStatus;
namespace Domain.Entities
{
    public class Task : BaseEntity
    {
        private readonly List<Comment> _comments = [];
        public IReadOnlyCollection<Comment> Comments { get { return _comments.AsReadOnly(); } }
        public ICollection<TaskAssignment> TaskAssignments { get; set; } = [];

        public string TaskName { get; private set; } = default!;
        public string TaskDescription { get; private set; } = default!;
        public DateTimeOffset StartDate { get; private set; }

        public DateTimeOffset EndDate { get; private set; }
        public TaskStatus TaskStatus { get; private set; } = TaskStatus.Todo;

        public int ProjectId { get; private set; }
        public Project Project { get; set; } = default!;
        private Task() { }
        internal Task(Guid taskPublicId, string taskName, string taskDescription, DateTimeOffset startDate, DateTimeOffset endDate, TaskStatus taskStatus, int projectId)
        {
            if (string.IsNullOrWhiteSpace(taskName))
                throw new InvalidArgument($"Task name can't be null or empty. {nameof(taskName)}");
            if (string.IsNullOrWhiteSpace(taskDescription))
                throw new InvalidArgument($"Task description can't be null or empty. {nameof(taskDescription)}");
            if (startDate > endDate)
                throw new InvalidArgument($"Start date cannot be later than end date. {nameof(startDate)}");
            PublicId = taskPublicId;
            TaskName = taskName;
            TaskDescription = taskDescription;
            StartDate = startDate;
            EndDate = endDate;
            ProjectId = projectId;
            TaskStatus = taskStatus;
        }

        public Comment AddComment(string content , int authorId)
        {
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(content.Trim()))
                throw new InvalidArgument($"Comment cannot be null or empty. {nameof(content)}");
            if(_comments.Any(x => x.Content.Trim() == content.Trim()))
                throw new InvalidOperation("Comment already exist.");
            if (TaskStatus == TaskStatus.Cancelled || TaskStatus == TaskStatus.Completed || EndDate < DateTimeOffset.UtcNow)
                throw new InvalidOperation("omments cannot be added to a task that is completed, cancelled, or past its deadline.");
            var comment = new Comment(content, Id,authorId);
            _comments.Add(comment);
            return comment;
        }
        public void UpdateComment(Guid commentId, string content)
        {
            if (string.IsNullOrWhiteSpace(content) || string.IsNullOrWhiteSpace(content.Trim()))
                throw new InvalidArgument($"Comment cannot be null or empty. {nameof(content)}");
            if (TaskStatus == TaskStatus.Cancelled || TaskStatus == TaskStatus.Completed || EndDate < DateTimeOffset.UtcNow)
                throw new InvalidOperation("omments cannot be added to a task that is completed, cancelled, or past its deadline.");
            var comment = _comments.FirstOrDefault(x => x.PublicId == commentId) ?? throw new InvalidOperation("The specified comment does not exist in this task.");
            if (comment.Content.Trim() == content.Trim())
                return;
            comment.Update(content);
        }

        internal void Update(string taskName, string taskDescription, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (string.IsNullOrWhiteSpace(taskName))
                throw new InvalidArgument($"Task name can't be null or empty. {nameof(taskName)}");
            if (string.IsNullOrWhiteSpace(taskDescription))
                throw new InvalidArgument($"Task description can't be null or empty. {nameof(taskDescription)}");
            if (startDate > endDate)
                throw new InvalidArgument($"Start date cannot be later than end date. {nameof(startDate)}");
            TaskName = taskName;
            TaskDescription = taskDescription;
            StartDate = startDate;
            EndDate = endDate;
        }
        internal void Start()
        {
            if (TaskStatus != TaskStatus.Todo)
                throw new InvalidOperation("Task can only be started if it is in the 'Todo' status.");
            TaskStatus = TaskStatus.InProgress;
        }
        internal void PutInReview()
        {
            if (TaskStatus != TaskStatus.InProgress)
                throw new InvalidOperation("Task can only be put in review if it is in the 'InProgress' status.");
            TaskStatus = TaskStatus.InReview;
        }
        internal void Complete()
        {
            if (TaskStatus != TaskStatus.InProgress && TaskStatus != TaskStatus.InReview)
                throw new InvalidOperation("Task can only be completed if it is in the 'InProgress' or 'InReview' status.");
            TaskStatus = TaskStatus.Completed;
        }
        internal void Cancel()
        {
            if (TaskStatus == TaskStatus.Cancelled || TaskStatus == TaskStatus.Completed)
                throw new InvalidOperation("Task cannot be cancelled from the Completed or Cancelled status.");
            TaskStatus = TaskStatus.Cancelled;
        }
        internal void PutOnHold()
        {
            if (TaskStatus == TaskStatus.Completed || TaskStatus == TaskStatus.Cancelled)
                throw new InvalidOperation("Task cannot be put on hold from the Completed or Cancelled status.");
            TaskStatus = TaskStatus.OnHold;
        }

        internal void Resume()
        {
            if (TaskStatus != TaskStatus.OnHold)
                throw new InvalidOperation("Task can only be resumed if it is in the 'OnHold' status.");
            TaskStatus = TaskStatus.InProgress;
        }


    }
}
