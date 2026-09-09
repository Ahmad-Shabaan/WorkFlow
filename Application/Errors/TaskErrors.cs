using Application.Common.Errors;

namespace Application.Errors
{
    public static class TaskErrors
    {
        public static readonly NotFoundError NotFoundError = new("Task.NotFound", "Task with id not found.");

    }
}
