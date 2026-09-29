namespace Domain.Entities
{
    public class TaskAssignment
    {
        public int UserId { get; set; }
        public int TaskId { get; set; }
        public Task Task { get; set; } = default!;
    }
}
