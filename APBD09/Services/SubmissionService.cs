using APBD09.Data;
using APBD09.DTOs;
using APBD09.Exceptions;
using APBD09.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD09.Services;

public class SubmissionService
{
    private readonly UniversityTasksDbContext _context;
        
    public SubmissionService(UniversityTasksDbContext context)
    {
        _context = context;
    }
    
    public async Task<int> CreateSubmissionAsync(CreateSubmissionDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.RepositoryUrl))
            throw new BadRequestException("Repository URL cannot be empty.");
        if (!dto.RepositoryUrl.StartsWith("https://"))
            throw new BadRequestException("Repository URL must start with 'https://'.");
        
        var student = await _context.Students.FindAsync(dto.StudentId);
        if (student == null)
            throw new NotFoundException($"Student with id {dto.StudentId} not found.");
        if (!student.IsActive)
            throw new BadRequestException($"Student with id {dto.StudentId} is not active.");
        
        var assignment = await _context.Assignments.FindAsync(dto.AssignmentId);
        if (assignment == null)
            throw new NotFoundException($"Assignment with id {dto.AssignmentId} not found.");
        if (!assignment.IsPublished)
            throw new BadRequestException($"Assignment with id {dto.AssignmentId} is not published");
        
        var enrollement = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.StudentId == dto.StudentId && e.CourseId == assignment.CourseId);
        if (enrollement == null)
            throw new BadRequestException($"Student with id {dto.StudentId} is not enrolled in course with id {assignment.CourseId}.");
        if (enrollement.Status != "Active" && enrollement.Status != "Completed")
            throw new BadRequestException($"Enrollment status must be 'Active' or 'Completed' to submit an assignment.");
        
        var existingSubmission = await _context.Submissions
            .FirstOrDefaultAsync(s => s.StudentId == dto.StudentId && s.AssignmentId == dto.AssignmentId);
        if (existingSubmission != null)
            throw new ConflictException($"Student with id {dto.StudentId} has already submitted for assignment with id {dto.AssignmentId}.");
        
        var now  = DateTime.UtcNow;
        var submission = new Submission
        {
            StudentId = dto.StudentId,
            AssignmentId = dto.AssignmentId,
            RepositoryUrl = dto.RepositoryUrl,
            SubmittedAt = now,
            Status = assignment.IsOverdue(now) ? "Late" : "Submitted"
        };
        
        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();
        
        return submission.AssignmentId;
    }

    public async Task GradeSubmissionAsync(int id, GradeSubmissionDto dto)
    {
        var submission = await _context.Submissions
            .Include(s => s.Assignment)
            .FirstOrDefaultAsync(s => s.AssignmentId == id);
        if (submission == null)
            throw new NotFoundException($"Submission with id {id} not found.");
        if (dto.Score < 0 || dto.Score > submission.Assignment.MaxPoints)
            throw new BadRequestException($"Score must be between 0 and {submission.Assignment.MaxPoints}.");
        
        submission.Score = dto.Score;
        submission.Feedback = dto.Feedback;
        submission.Status = "Graded";
        await _context.SaveChangesAsync();
    }
    
    public async Task DeleteSubmissionAsync(int id)
    {
        var submission = await _context.Submissions.FindAsync(id);
        
        if (submission == null)
            throw new NotFoundException($"Submission with id {id} not found.");
        
        if (submission.Status == "Graded")
            throw new BadRequestException($"A graded submission cannot be deleted.");
        
        _context.Submissions.Remove(submission);
        await _context.SaveChangesAsync();
    }
}