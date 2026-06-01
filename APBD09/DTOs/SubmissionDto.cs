namespace APBD09.DTOs;

public class SubmissionDto
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int AssignmentId { get; set; }
    public string RepositoryUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public int? Score { get; set; }
    public string? Feedback { get; set; }
}