namespace Application.Interfaces.BackgroundJobs.Hangfire
{
    public interface ITaskNotificationScheduler
    {
        void EnqueueTaskCompletedNotification(Guid taskId, CancellationToken cancellationToken);
    }
}
