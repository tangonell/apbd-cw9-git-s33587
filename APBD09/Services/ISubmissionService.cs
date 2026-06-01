using APBD09.DTOs;

namespace APBD09.Services;

public interface ISubmissionService
{
    public Task<int> CreateSubmissionAsync(CreateSubmissionDto dto);
    public Task GradeSubmissionAsync(int id, GradeSubmissionDto dto);
    public Task DeleteSubmissionAsync(int id);
}