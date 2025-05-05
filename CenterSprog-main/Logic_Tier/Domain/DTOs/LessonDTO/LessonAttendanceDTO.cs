namespace Domain.DTOs.LessonDTO;

public class LessonAttendanceDTO
{
    public long Date { get; init; }
    public string Topic { get; init; }
    public bool HasAttended { get; init; }

}