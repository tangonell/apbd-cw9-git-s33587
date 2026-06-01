using APBD09.Data;
using APBD09.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD09.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly UniversityTasksDbContext _context;
        
        public StudentsController(UniversityTasksDbContext context)
        {
            _context = context;
        }
        
        [HttpGet("{id:int}/dashboard")]
        public async Task<IActionResult> GetStudentDashboard(int id)
        {
            // Using projection to avoid the n+1 problem
            var dashboard = await _context.Students
                .Where(s => s.StudentId == id)
                .Select(s => new StudentDashboardDto
                {
                    StudentId = s.StudentId,
                    IndexNumber = s.IndexNumber,
                    FullName = s.FullName,
                    IsActive = s.IsActive,
                    Enrollments = s.Enrollments.Select(e => new StudentEnrollmentDto
                    {
                        EnrollmentId = e.EnrollmentId,
                        CourseId = e.CourseId,
                        EnrolledAt = e.EnrolledAt,
                        Status = e.Status
                    }).ToList(),
                    Submissions = s.Submissions.Select(s => new StudentSubmissionDto
                    {
                        SubmissionId = s.SubmissionId,
                        AssignmentId = s.AssignmentId,
                        RepositoryUrl = s.RepositoryUrl,
                        SubmittedAt = s.SubmittedAt,
                        Status = s.Status,
                        Score = s.Score,
                        Feedback = s.Feedback
                    }).ToList()
                })
                .FirstOrDefaultAsync();
            if (dashboard == null) return NotFound($"Student with id {id} not found");
            return Ok(dashboard);
        }
    }
}
