using Domain.Common;
using Domain.Enums;
using Domain.Exceptions;
using TaskStatus = Domain.Enums.TaskStatus;
namespace Domain.Entities.ProjectAggregate
{
    public class Project : BaseAggregateRoot
    {
        private readonly List<Task> _items = [];

        public string ProjectName { get; private set; } = default!;
        public string ProjectDescription { get; private set; } = default!;
        public DateTimeOffset StartDate { get; private set; }
        public DateTimeOffset EndDate { get; private set; }
        public ProjectStatus ProjectStatus { get; private set; } = ProjectStatus.Planning;
        private Project() { }
        public Project(string projectName, string projectDescription, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (string.IsNullOrWhiteSpace(projectName))
                throw new InvalidArgument($"Project name can't be null or empty. {nameof(projectName)}");
            if (string.IsNullOrWhiteSpace(projectDescription))
                throw new InvalidArgument($"Project description can't be null or empty. {nameof(projectDescription)}");
            if (startDate > endDate)
                throw new InvalidArgument($"Start date cannot be later than end date. {nameof(startDate)}");
            if (endDate < startDate)
                throw new InvalidArgument("End date cannot be earlier than start date.");
            ProjectName = projectName;
            ProjectDescription = projectDescription;
            StartDate = startDate;
            EndDate = endDate;
        }
        public IReadOnlyCollection<Task> Tasks { get { return _items; } }

        public Task AddTask(string taskName, string taskDescription, DateTimeOffset startDate, DateTimeOffset endDate, Guid? taskPublicId, TaskStatus taskStatus = TaskStatus.Todo)
        {
            // Add validation logic here if needed based on business rules, for example, you might want to check if the task dates are within the project dates.
            if (startDate > EndDate)
                throw new InvalidArgument("Task start date cannot be later than project end date.");
            if (startDate < StartDate)
                throw new InvalidArgument("Task start date cannot be earlier than project start date.");
            if (endDate > EndDate)
                throw new InvalidArgument("Task cannot end after the project.");
            taskPublicId ??= Guid.NewGuid();

            var task = new Task(taskPublicId.Value, taskName, taskDescription, startDate, endDate, taskStatus, Id);
            _items.Add(task);
            return task;
        }


        public void UpdateTask(Guid publicTaskId, string taskName, string taskDescription, DateTimeOffset startDate, DateTimeOffset endDate)
        {
            if (startDate > EndDate)
                throw new InvalidArgument("Task start date cannot be later than project end date.");
            if (startDate < StartDate)
                throw new InvalidArgument("Task start date cannot be earlier than project start date.");
            if (endDate > EndDate)
                throw new InvalidArgument("Task cannot end after the project.");
            var task = _items.FirstOrDefault(t => t.PublicId == publicTaskId) ?? throw new InvalidOperation("The specified task does not exist in this project.");
            task.Update(taskName, taskDescription, startDate, endDate);

        }
        public void DeleteTask(Guid publicTaskId)
        {
            var task = _items.FirstOrDefault(t => t.PublicId == publicTaskId) ?? throw new InvalidOperation("The specified task does not exist in this project.");
            _items.Remove(task);
        }
        public void AddCommentToTask(Guid publicTaskId, string commentContent)
        {
            var task = _items.FirstOrDefault(t => t.PublicId == publicTaskId) ?? throw new InvalidOperation("The specified task does not exist in this project.");
            task.AddComment(commentContent);
        }

        public void Start()
        {
            if (ProjectStatus != ProjectStatus.Planning)
                throw new InvalidOperation("Project can only be started from the Planning status.");
            ProjectStatus = ProjectStatus.InProgress;
            foreach (var item in _items)
            {
                item.Start();
            }
        }
        public void PutOnHold()
        {
            if (ProjectStatus != ProjectStatus.InProgress)
                throw new InvalidOperation("Project can only be put on hold from the In Progress status.");
            foreach (var item in _items)
            {
                if (item.TaskStatus == TaskStatus.InProgress)
                    item.PutOnHold();
            }
            ProjectStatus = ProjectStatus.OnHold;

        }
        public void Complete()
        {
            if (ProjectStatus != ProjectStatus.InProgress && ProjectStatus != ProjectStatus.OnHold)
                throw new InvalidOperation("Project can only be completed from the In Progress or On Hold status.");
            foreach (var task in _items)
            {
                if (task.TaskStatus != TaskStatus.Completed)
                    throw new InvalidOperation("All tasks must be completed before the project can be marked as completed.");
            }
            ProjectStatus = ProjectStatus.Completed;
        }

        public void Cancel()
        {
            if (ProjectStatus == ProjectStatus.Completed || ProjectStatus == ProjectStatus.Cancelled)
                throw new InvalidOperation("Project cannot be cancelled from the Completed or Cancelled status.");
            foreach (var item in _items)
            {
                if (item.TaskStatus == TaskStatus.Completed)
                    continue;
                item.Cancel();
            }
            ProjectStatus = ProjectStatus.Cancelled;
        }

        public void Resume()
        {
            if (ProjectStatus != ProjectStatus.OnHold)
                throw new InvalidOperation("Project can only be resumed from the On Hold status.");
            ProjectStatus = ProjectStatus.InProgress;
            foreach (var item in _items)
            {
                if (item.TaskStatus == TaskStatus.OnHold)
                    item.Resume();
            }
        }
        public void PutTaskInReview(Guid taskId)
        {
            var task = _items.FirstOrDefault(t => t.PublicId == taskId) ?? throw new InvalidOperation("The specified task does not exist in this project.");
            task.PutInReview();
        }

        public void CompleteTask(Guid taskId)
        {
            var task = _items.FirstOrDefault(t => t.PublicId == taskId) ?? throw new InvalidOperation("The specified task does not exist in this project.");
            task.Complete();
        }
        public void CancelTask(Guid taskId)
        {
            var task = _items.FirstOrDefault(t => t.PublicId == taskId) ?? throw new InvalidOperation("The specified task does not exist in this project.");
            task.Cancel();
        }
    }
}
