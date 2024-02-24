using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class NotFoundAssignmentException : Xeption
{
    public NotFoundAssignmentException(Guid assignmentId)
        : base($"Assignment is not found in {assignmentId} id")
    { }
}