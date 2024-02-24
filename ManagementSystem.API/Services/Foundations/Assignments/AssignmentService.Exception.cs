using EFxceptions.Models.Exceptions;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;
using Microsoft.Data.SqlClient;
using Xeptions;

namespace ManagementSystem.API.Services.Foundations.Assignments;

public partial class AssignmentService
{
    private delegate ValueTask<Assignment> ReturningAssignmentFunction();

    private delegate IQueryable<Assignment> ReturningAllAssignmentsFunction();

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
        catch (InvalidAssignmentException invalidAssignmentException)
        {
            throw CreateAndLogValidationException(invalidAssignmentException);
        }
        catch (DuplicateKeyException duplicateKeyException)
        {
            var alreadyExistsAssignmentException =
                new AlreadyExistsAssignmentException(duplicateKeyException);

            throw CreateAndLogValidationException(alreadyExistsAssignmentException);
        }
        catch (SqlException sqlException)
        {
            var failedAssignmentStorageException =
                new FailedAssignmentStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedAssignmentStorageException);
        }
        catch (Exception exception)
        {
            var failedAssignmentServiceException =
                new FailedAssignmentServiceException(exception);

            throw CreateAndLogServiceException(failedAssignmentServiceException);
        }
    }

    private IQueryable<Assignment> TryCatch(ReturningAllAssignmentsFunction returningAllAssignmentsFunction)
    {
        try
        {
            return returningAllAssignmentsFunction();
        }
        catch (SqlException sqlException)
        {
            var failedAssignmentStorageException =
                new FailedAssignmentStorageException(sqlException);

            throw CreateAndLogCriticalDependencyException(failedAssignmentStorageException);
        }
    }

    private Exception CreateAndLogValidationException(Xeption exception)
    {
        var assignmentValidationException = 
            new AssignmentValidationException(exception);
        
        this.loggingBroker.LogError(assignmentValidationException);
        return assignmentValidationException;
    }
    
    private Exception CreateAndLogCriticalDependencyException(Xeption exception)
    {
        var assignmentDependencyException = 
            new AssignmentDependencyException(exception);
        
        this.loggingBroker.LogCritical(assignmentDependencyException);
        return assignmentDependencyException;
    }

    private Exception CreateAndLogServiceException(Exception exception)
    {
        var assignmentServiceException = 
            new AssignmentServiceException(exception);

        this.loggingBroker.LogError(assignmentServiceException);
        return assignmentServiceException;
    }
}