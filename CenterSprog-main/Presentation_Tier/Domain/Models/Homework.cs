namespace Domain.Models;

public class Homework
{
    public string Id { get; set; }
    public long Deadline { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }

    public Homework(string id, long deadline, string title, string description)
    {
        Id = id;
        Deadline = deadline;
        Title = title;
        Description = description;
    }

    public Homework()
    {
    }
}