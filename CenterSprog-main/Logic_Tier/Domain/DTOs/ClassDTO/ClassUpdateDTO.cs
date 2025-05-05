using Domain.Models;

namespace Domain.DTOs.ClassDTO;

public class ClassUpdateDTO
{
    public string Id { get; set; }
    public string? Room { get; set; }
    public string? Title { get; set; }
    public List<string>? Participants { get; set; }

    public ClassUpdateDTO()
    {
    }

    public ClassUpdateDTO(string id)
    {
        Participants = new List<string>();
        Id = id;
    }

    public ClassUpdateDTO(string id, string room, string title)
    {
        Participants = new List<string>();
        Id = id;
        Room = room;
        Title = title;
    }
}