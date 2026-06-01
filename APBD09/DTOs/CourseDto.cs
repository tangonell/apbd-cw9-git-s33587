namespace APBD09.DTOs;

public class CourseDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int Credits { get; set; }
    public int AssignmentCount { get; set; }
}