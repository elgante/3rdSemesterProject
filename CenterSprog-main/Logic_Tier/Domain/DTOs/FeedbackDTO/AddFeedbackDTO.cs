namespace Domain.DTOs.FeedbackDTO;

public class AddFeedbackDTO
{
    public string? HandInId { get; init; }
    public string? StudentUsername { get; init; }
    public int Grade { get; init; }
    public string? Comment { get; init; }
    
}