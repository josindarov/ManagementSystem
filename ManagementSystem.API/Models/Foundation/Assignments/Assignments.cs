namespace ManagementSystem.API.Models.Foundation.Assignments;

public class Assignments
{
    public Guid Id { get; set; }

    public string? Title { get; set; }

    public string? Description { get; set; }

    public DateTime DueDate { get; set; }

    public string? TaskPriority { get; set; }

    public string? State { get; set; }

    public string? Note { get; set; }
}