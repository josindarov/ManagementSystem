using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

namespace ManagementSystem.API.Services.Foundations.Assignments;

public partial class AssignmentService
{
    private static void ValidateAssignmentOnAdd(Assignment assignment)
    {
        ValidateAssignmentNotNull(assignment);
    }
    
    private static void ValidateAssignmentNotNull(Assignment assignment)
    {
        if (assignment is null)
        {
            throw new NullAssignmentException();
        }
    }
}