using Application.Common.Errors;

namespace Application.Errors
{
    public static class ProjectErrors
    {
        public static readonly NotFoundError NotFoundError = new("Project.NotFound", "Project with id not found.");
        public static readonly ConflictError ConflictError = new("Project.Confilct", "Project can be only managed by one manager.");
        public static readonly ConflictError MaximumProjects = new("Project.Confilct", "This manager already manages another project. Please choose anohter one.");

        public static readonly ConflictError TaskAlreadyExistError = new("Project.Confilct", "Project already has task with this name.");
        public static readonly ConflictError NotManager = new("Project.NotManager", "Project can not be assigned to any user.");


    }
}
