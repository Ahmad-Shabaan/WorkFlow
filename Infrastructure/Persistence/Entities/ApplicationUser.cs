using Domain.Entities;
using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Persistence.Entities
{
    public class ApplicationUser : IdentityUser<int>
    {
        public Guid PublicId { get; protected set; } = Guid.NewGuid();
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;

        public string FullName => $"{FirstName} {LastName}";

        public Project? Project { get; set; }
        public ICollection<TaskAssignment> TaskAssignments { get; set; } = [];
        public ICollection<Comment> Comments { get; set; } = [];

    }
}
