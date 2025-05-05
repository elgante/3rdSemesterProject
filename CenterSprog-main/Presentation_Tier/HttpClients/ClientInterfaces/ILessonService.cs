using Domain.DTOs.LessonDTO;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface ILessonService
{
    Task<Lesson> GetByIdAsync(string jwt, string id);
    Task<string> MarkAttendanceAsync(string jwt, MarkAttendanceDTO markAttendanceDto);
    Task<IEnumerable<User>> GetAttendanceAsync(string jwt, string id);
    Task<Lesson> CreateAsync(string jwt, LessonCreationDTO lessonCreationDto);
    Task UpdateLessonAsync(string jwt, LessonUpdateDTO updateDto);
    Task DeleteAsync(string jwt, string lessonId);

}