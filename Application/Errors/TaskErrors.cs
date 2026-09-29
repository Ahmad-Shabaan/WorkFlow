using Application.Common.Errors;

namespace Application.Errors
{
    public static class TaskErrors
    {
        public static readonly NotFoundError NotFoundError =
            new("Task.NotFound", "The specified task was not found.");

        public static readonly ConflictError UserNotEmployee =
            new("Task.UserNotEmployee", "Only employees can be assigned to tasks.");

        public static readonly ConflictError AlreadyAssigned =
            new("Task.AlreadyAssigned", "The employee is already assigned to this task.");

        public static readonly ConflictError MaximumActiveTasks =
            new("Task.MaximumActiveTasks", "The employee cannot be assigned to more than 2 active tasks.");
    }
}
