using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class AssignmentServiceException : Xeption
{
    public AssignmentServiceException(Exception innerException)
        : base("Service error occured, fix it and try again.", innerException)
    { }
}