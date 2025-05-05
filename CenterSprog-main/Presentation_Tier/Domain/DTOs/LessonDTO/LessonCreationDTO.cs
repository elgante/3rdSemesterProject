namespace Domain.DTOs.LessonDTO;
public class LessonCreationDTO
{
    public long Date{ get; set; }
    public string? ClassId { get; init; }
    public string? Topic{ get; set; }
    public string? Description{ get; set; }
}