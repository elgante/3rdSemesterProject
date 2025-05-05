using Domain.DTOs.LessonDTO;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface ILessonLogic
{
    Task<Lesson> GetByIdAsync(string id);
    Task<int> MarkAttendanceAsync(MarkAttendanceDTO markAttendanceDto);
    Task<IEnumerable<User>> GetAttendanceAsync(string id);
    Task<Lesson> CreateAsync(LessonCreationDTO lessonCreationDto);
    Task<Boolean> UpdateLessonAsync(LessonUpdateDTO lessonUpdateDto);
    Task<Boolean> DeleteAsync(string lessonId);
}