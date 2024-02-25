using System.Data;
using ManagementSystem.API.Models.Foundation.Assignments;
using ManagementSystem.API.Models.Foundation.Assignments.Exceptions;

namespace ManagementSystem.API.Services.Foundations.Assignments;

public partial class AssignmentService
{
    private static void ValidateAssignmentOnAdd(Assignment assignment)
    {
        ValidateAssignmentNotNull(assignment);
        Validate(
            (Rule: IsInvalid(assignment.Id), Parameter: nameof(Assignment.Id)),
            (Rule: IsInvalid(assignment.Description), Parameter: nameof(Assignment.Description)),
            (Rule: IsInvalid(assignment.Title), Parameter: nameof(Assignment.Title)),
            (Rule: IsInvalid(assignment.Note), Parameter: nameof(Assignment.Note)),
            (Rule: IsInvalid(assignment.State), Parameter: nameof(Assignment.State)),
            (Rule: IsInvalid(assignment.TaskPriority), Parameter: nameof(Assignment.TaskPriority)),
            (Rule: IsInvalid(assignment.DueDate), Parameter: nameof(Assignment.DueDate))
            );
    }

    private static void ValidateAssignmentOnModify(Assignment assignment)
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

    private static void ValidateStoreAssignment(Assignment assignment, Guid assignmentId)
    {
        if (assignment is null)
        {
            throw new NotFoundAssignmentException(assignmentId);
        }
    }
    
    private static void ValidateAssignmentId(Guid id) =>
        Validate((Rule: IsInvalid(id), Parameter: nameof(Assignment.Id)));

    private static dynamic IsInvalid(string text) => new
    {
        Condition = string.IsNullOrWhiteSpace(text),
        Message = "Text is required"
    };
    
    private static dynamic IsInvalid(DateTimeOffset date) => new
    {
        Condition = date == default,
        Message = "Date is required"
    };
    
    private static dynamic IsInvalid(Guid id) => new
    {
        Condition = id == Guid.Empty,
        Message = "Id is required"
    };

    private static void Validate(params (dynamic Rule, string Parameter)[] validations)
    {
        var invalidAssignmentException = new InvalidAssignmentException();

        foreach ((dynamic rule, string parameter) in validations)
        {
            if (rule.Condition)
            {
                invalidAssignmentException.UpsertDataList(
                    key: parameter,
                    value: rule.Message);
            }
        }

        invalidAssignmentException.ThrowIfContainsErrors();
    }
}