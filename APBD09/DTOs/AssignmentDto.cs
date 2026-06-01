namespace APBD09.DTOs;

public class AssignmentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public int MaxPoints { get; set; }
    public bool IsPublished { get; set; }
    public int SubmissionCount { get; set; }
}