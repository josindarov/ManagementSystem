using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class NullAssignmentException : Xeption
{
    public NullAssignmentException()
        : base("Assignment is null.")
    { }
}