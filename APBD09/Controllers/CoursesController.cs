using APBD09.Data;
using APBD09.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBD09.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly UniversityTasksDbContext _context;
        
        public CoursesController(UniversityTasksDbContext context)
        {
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetCourses([FromQuery] bool activeOnly = true)
        {
            var query = _context.Courses.AsNoTracking();
            if (activeOnly) query = query.Where(c => c.IsActive);
            var courses = await query.Select(c => new CourseDto {
                Id  = c.CourseId,
                Code = c.Code,
                Name = c.Name,
                Credits = c.Credits,
                AssignmentCount = c.Assignments.Count()
            })
            .ToListAsync();
            return Ok(courses);
        }

        [HttpGet("{id:int}/assignments")]
        public async Task<IActionResult> GetCourseAssignments(int id, [FromQuery] bool publishedOnly = true)
        {
            var courseExists = await _context.Courses.AnyAsync(c => c.CourseId == id);
            if (!courseExists) return NotFound($"Course with id {id} not found");
            
            var query = _context.Assignments.AsNoTracking();
            if (publishedOnly) query = query.Where(a => a.IsPublished);
            
            var assignments = await query.Where(a => a.CourseId == id)
                .Select(a => new AssignmentDto {
                    Id = a.AssignmentId,
                    Title = a.Title,
                    DueDate = a.DueDate,
                    MaxPoints = a.MaxPoints,
                    IsPublished = a.IsPublished,
                    SubmissionCount = a.Submissions.Count()
                })
                .ToListAsync();
            return Ok(assignments);
        }
    }
}
