using Domain.Common;
namespace Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; private set; } = default!;
        public int TaskId { get; private set; }
        public Task Task { get; private set; } = default!;
        public int AuthorId { get; private set; }
        private Comment() { }
        internal Comment(string content, int taskId,int authorId)
        {
            Content = content;
            TaskId = taskId;
            AuthorId = authorId;
        }
        internal void Update(string content)
        {
            Content = content;
        }
    }
}
