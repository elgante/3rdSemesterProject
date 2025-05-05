using Domain.Models;

namespace Domain.DTOs.HomeworkDTO;

public class HomeworkHandInDTO
{
    public string? HomeworkId { get; init; }
    public string? StudentUsername { get; init; }
    public string? Answer { get; init; }
    
}