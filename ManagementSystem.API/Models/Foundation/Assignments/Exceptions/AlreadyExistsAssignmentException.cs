using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class AlreadyExistsAssignmentException : Xeption
{
    public AlreadyExistsAssignmentException(Exception innerException)
        : base("Assignment already exists.", innerException)
    { }
}