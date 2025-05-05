namespace Domain.Models;

public class Feedback
{
    public string Id { get; set; }
    public int Grade { get; set; }
    public string Comment { get; set; }

    public Feedback(string id, int grade, string comment)
    {
        Id = id;
        Grade = grade;
        Comment = comment;
    }

    public Feedback()
    {
        Id = "No id";
        Grade = -1;
        Comment = "No comment";
    }
}