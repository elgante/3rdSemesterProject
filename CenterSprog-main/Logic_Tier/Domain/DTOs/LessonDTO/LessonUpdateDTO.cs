namespace Domain.DTOs.LessonDTO;

public class LessonUpdateDTO
{
   public string? Id { get; init; }
    
    public long Date{ get; init; }
   
    public string? Topic{ get; init; }
    
    public string? Description{ get; init; }
    
}