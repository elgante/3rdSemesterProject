namespace Domain.DTOs.LessonDTO;

public class MarkAttendanceDTO
{
    public string LessonId { get; init; }
    public List<String> StudentUsernames { get; init; }
}