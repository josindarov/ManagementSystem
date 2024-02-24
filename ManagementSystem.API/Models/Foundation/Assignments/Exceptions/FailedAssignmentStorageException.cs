using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class FailedAssignmentStorageException : Xeption
{
    public FailedAssignmentStorageException(Exception innerException)
        : base("Failed Assignment error occured, contact support.", innerException)
    { }
}