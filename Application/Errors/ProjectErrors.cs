using Application.Common.Errors;

namespace Application.Errors
{
    public static class ProjectErrors
    {
        public static readonly NotFoundError NotFoundError = new("Project.NotFound", "Project with id not found.");
    }
}
