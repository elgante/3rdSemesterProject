namespace Domain.DTOs.UserDTO;

public class UserAttendanceDTO
{
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public string Email { get; init; }
    public double TotalAbsence { get; set; }
}