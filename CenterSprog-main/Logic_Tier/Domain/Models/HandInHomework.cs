namespace Domain.Models;

public class HandInHomework
{
    public string Id { get; set; }
    public string Answer { get; set; }
    public string StudentUsername { get; set; }
    
    public Feedback? Feedback { get; set; }

    public HandInHomework(string id, string answer)
    {
        Id = id;
        Answer = answer;
        Feedback = null;
    }

    public HandInHomework()
    {
        Feedback = null;
    }

    public HandInHomework(string id, string answer, string studentUsername)
    {
        Id = id;
        Answer = answer;
        StudentUsername = studentUsername;
        Feedback = null;
    }

    public HandInHomework(string id, string answer, string studentUsername, Feedback? feedback)
    {
        Id = id;
        Answer = answer;
        StudentUsername = studentUsername;
        Feedback = feedback;
    }
}