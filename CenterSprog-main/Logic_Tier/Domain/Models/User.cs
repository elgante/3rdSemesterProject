namespace Domain.Models;

public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }

    public User() { }

    public User(string username, string password, string firstName, string lastName, string email, string role)
    {
        Username = username;
        Password = password;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Role = role;
    }

    public User(string username, string firstName, string lastName, string role)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }
    public User(string username, string firstName, string lastName)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
    }

    public User(string username, string firstName, string lastName, string role, string email)
    {
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        Email = email;
    }
}