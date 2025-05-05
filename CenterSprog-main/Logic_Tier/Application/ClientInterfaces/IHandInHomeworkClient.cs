using Domain.DTOs.HomeworkDTO;
using Domain.Models;

namespace Application.ClientInterfaces;

public interface IHandInHomeworkClient
{
    Task<HandInHomework> HandInHomework(HomeworkHandInDTO dto);
    Task<IEnumerable<HandInHomework>> GetHandInsByHomeworkIdAsync(string homeworkId);
    Task<HandInHomework> GetHandInByHomeworkIdAndStudentUsernameAsync(string homeworkId, string studentUsername);
}