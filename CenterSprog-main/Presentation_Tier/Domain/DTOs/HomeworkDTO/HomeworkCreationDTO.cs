namespace Domain.DTOs.HomeworkDTO;

public class HomeworkCreationDTO
{
    public long Deadline { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? LessonId { get; init; }
}