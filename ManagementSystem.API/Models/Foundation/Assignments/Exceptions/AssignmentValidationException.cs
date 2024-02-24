using Xeptions;

namespace ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

public class AssignmentValidationException : Xeption
{
    public AssignmentValidationException(Xeption innerException)
        : base("Assingment validation error occured, fix it and try again.", innerException)
    { }
}