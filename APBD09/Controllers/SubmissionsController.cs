using APBD09.DTOs;
using APBD09.Exceptions;
using APBD09.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD09.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubmissionsController : ControllerBase
    {
        private readonly ISubmissionService _submissionService;
        
        public SubmissionsController(ISubmissionService submissionService)
        {
            _submissionService = submissionService;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateSubmission([FromBody] CreateSubmissionDto dto)
        {
            try
            {
                var submissionId = await _submissionService.CreateSubmissionAsync(dto);
                return CreatedAtAction(nameof(CreateSubmission), new { id = submissionId }, null);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpPut("{id:int}/grade")]
        public async Task<IActionResult> GradeSubmission(int id, [FromBody] GradeSubmissionDto dto)
        {
            try
            {
                await _submissionService.GradeSubmissionAsync(id, dto);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteSubmission(int id)
        {
            try
            {
                await _submissionService.DeleteSubmissionAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
