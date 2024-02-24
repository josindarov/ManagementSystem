using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class FailedAssignmentServiceException : Xeption
{
    public FailedAssignmentServiceException(Exception innerException)
        : base("Failed Assignment service error occured, contact support.", innerException)
    { }
}