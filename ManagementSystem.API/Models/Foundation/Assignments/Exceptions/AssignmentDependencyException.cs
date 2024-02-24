using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class AssignmentDependencyException : Xeption
{
    public AssignmentDependencyException(Exception innerException)
        : base("Dependency error occured, contact support.", innerException)
    { }
}