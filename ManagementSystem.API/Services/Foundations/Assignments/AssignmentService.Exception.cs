using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Xeptions;

namespace ManagementSystem.API.Services.Foundations.Assignments;

public partial class AssignmentService
{
    private delegate ValueTask<Assignment> ReturningAssignmentFunction();

    private async ValueTask<Assignment> TryCatch(ReturningAssignmentFunction returningAssignmentFunction)
    {
        try
        {
            return await returningAssignmentFunction();
        }
        catch (NullAssignmentException nullAssignmentException)
        {
            throw CreateAndLogValidationException(nullAssignmentException);
        }
    }

    private Exception CreateAndLogValidationException(Xeption exception)
    {
        var assignmentValidationException = 
            new AssignmentValidationException(exception);
        
        this.loggingBroker.LogError(assignmentValidationException);
        return assignmentValidationException;
    }
}