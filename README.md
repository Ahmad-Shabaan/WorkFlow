










### You can test end points from this link https://f4bdpyxlqm.apidog.io/ 
     ============================================================================
     ============================================================================
     ============================================================================







Repository interfaces are intentionally placed in the **Application layer** rather than the **Domain layer**. The Domain layer should remain focused on business rules, entities, value objects, and domain behavior, without taking dependencies on persistence abstractions that are not required by the domain itself.

In this design, repositories are primarily used by Application use cases/handlers to retrieve and persist data. Therefore, defining their contracts in Application keeps the abstractions close to the consumers that actually depend on them, while Infrastructure provides the implementations.

This approach is also used by various real-world and open-source projects, so placing repository interfaces in Application is a valid Clean Architecture variation and does not break the dependency rule or the existing architecture.

The dependency flow remains:

Domain → Application → Infrastructure

Infrastructure depends on Application to implement the repository contracts, while Application depends only on abstractions and does not depend on EF Core or other persistence implementations.






# Sprint 5 — Task Management Core

## Summary

Extend the existing Sprint 4 Task Management API without rebuilding the architecture. Keep the existing **Project, Task, and Comment** features and make the system business-aware, validated, authenticated, and authorized.

### 1. Task & Project Business Rules

* Keep all existing Project, Task, and Comment operations.
* Enforce the **Project → Task → Comment** relationship.
* A Task must belong to an existing, non-deleted Project.
* A Comment must belong to an existing, non-deleted Task.
* Require Project name, Task title, and Comment content.
* Keep business rules outside Controllers.

### 2. Task Status & Workflow

* Add `TaskStatus` to the Task entity:

  * `Todo`
  * `InProgress`
  * `Completed`
  * `Cancelled`
* Create a dedicated use case for status changes.
* Allow only these transitions:

  * `Todo → InProgress`
  * `InProgress → Completed`
  * `Todo → Cancelled`
  * `InProgress → Cancelled`
* Reject invalid transitions with an appropriate error.

### 3. Authentication

* Integrate ASP.NET Core Identity.
* Implement:

  * Register
  * Login
  * JWT Access Tokens
  * JWT Claims
  * Refresh Tokens
  * Logout with refresh-token invalidation
* Connect authenticated users to Projects and Tasks where appropriate.
* Keep JWT generation and authentication logic outside Controllers.

### 4. Authorization

* Introduce `Admin` and `User` roles.
* Protect secured endpoints with `[Authorize]`.
* Authenticated users can create Projects.
* Users can modify only Projects they own.
* Users can create Tasks only in Projects they own or are authorized to access.
* Apply ownership rules to Comments.
* Prevent users from modifying other users' resources.
* Use role-based authorization where appropriate.
* Use claims/policy-based authorization when access depends on resource ownership.

### 5. Validation & Error Handling

* Extend the existing Validation Pipeline from Sprint 4.
* Add FluentValidation for:

  * `CreateProject`
  * `UpdateProject`
  * `CreateTask`
  * `UpdateTask`
  * `CreateComment`
* Run validation through the existing `ValidationBehavior`.
* Use `Result<T>` for expected operation failures where appropriate.
* Implement consistent `ProblemDetails` responses.
* Add Global Exception Handling.
* Handle:

  * `400 Bad Request`
  * `401 Unauthorized`
  * `403 Forbidden`
  * `404 Not Found`
  * `409 Conflict`
  * `500 Internal Server Error`

## Definition of Done

* Sprint 4 architecture remains intact.
* Project, Task, and Comment remain the core domain features.
* Business rules prevent invalid states.
* Task status follows the defined workflow.
* JWT authentication works.
* Refresh Tokens and Logout are implemented.
* Authorization enforces ownership and roles.
* FluentValidation runs through the existing pipeline.
* API errors are handled consistently with `ProblemDetails`.
* Business rules are not implemented inside Controllers.
* The solution builds and runs successfully.

### Final Goal

By the end of Sprint 5, the application should be a **secured, validated, and business-aware Task Management API**, rather than a collection of simple CRUD endpoints.
