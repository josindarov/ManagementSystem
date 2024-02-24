using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class InvalidAssignmentException : Xeption
{
    public InvalidAssignmentException()
        : base("Assignment is invalid.")
    { }
}