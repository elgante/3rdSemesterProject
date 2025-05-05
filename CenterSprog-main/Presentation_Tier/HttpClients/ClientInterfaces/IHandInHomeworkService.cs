using Domain.DTOs.HomeworkDTO;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IHandInHomeworkService
{
    Task<HandInHomework> HandInHomework(string jwt, HomeworkHandInDTO dto);
    Task<IEnumerable<HandInHomework>> GetHandInsByHomeworkIdAsync(string jwt, string homeworkId);
    Task<HandInHomework> GetHandInByHomeworkIdAndStudentUsernameAsync(string jwt, string homeworkId, string studentUsername);
}