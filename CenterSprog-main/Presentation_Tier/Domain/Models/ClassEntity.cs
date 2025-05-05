namespace Domain.Models;

public class ClassEntity
{
    public String Id { get; set; }
    public String Title { get; set; }
    public String Room { get; set; }

    public List<User> Participants { get; set; }

    public List<Lesson> Lessons { get; set; }


    public ClassEntity(string id, string title, string room)
    {
        Participants = new List<User>();
        Lessons = new List<Lesson>();
        Id = id;
        Title = title;
        Room = room;
    }


    public ClassEntity()
    {
    }
}