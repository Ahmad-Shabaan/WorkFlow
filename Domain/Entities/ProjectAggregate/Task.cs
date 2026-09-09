using Domain.Common;
using Domain.Exceptions;
using TaskStatus = Domain.Enums.TaskStatus;
namespace Domain.Entities.ProjectAggregate
{
    public class Task : BaseEntity
    {
        private readonly List<Comment> _comments = [];
        public IReadOnlyCollection<Comment> Comments { get { return _comments.AsReadOnly(); } }
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
        internal void AddComment(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment) || string.IsNullOrWhiteSpace(comment.Trim()))
            {
                throw new InvalidArgument($"Comment cannot be null or empty. {nameof(comment)}");
            }
            _comments.Add(new Comment(comment, Id));
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
