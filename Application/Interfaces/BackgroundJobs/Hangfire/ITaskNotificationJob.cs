namespace Application.Interfaces.BackgroundJobs.Hangfire
{
    public interface ITaskNotificationJob
    {
        Task ExecuteAsync(Guid taskId, CancellationToken cancellationToken);
    }
}
