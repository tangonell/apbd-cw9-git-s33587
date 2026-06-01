namespace APBD09.DTOs;

public class StudentDashboardDto
{
    public int StudentId { get; set; }
    public string IndexNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public List<StudentSubmissionDto> Submissions { get; set; } = [];
}

public class StudentEnrollmentDto
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public DateOnly EnrolledAt { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class StudentSubmissionDto
{
    public int SubmissionId { get; set; }
    public int AssignmentId { get; set; }
    public string RepositoryUrl { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public int? Score { get; set; }
    public string? Feedback { get; set; }
}