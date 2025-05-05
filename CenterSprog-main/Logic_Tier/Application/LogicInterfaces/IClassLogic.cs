using Domain.DTOs.ClassDTO;
using Domain.DTOs.LessonDTO;
using Domain.DTOs.UserDTO;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IClassLogic
{
    Task<ClassEntity> GetByIdAsync(string id);
    Task<IEnumerable<ClassEntity>> GetAllAsync(SearchClassDTO dto);
    Task<IEnumerable<User>> GetAllParticipantsAsync(SearchClassParticipantsDTO dto);
    Task<ClassEntity> CreateAsync(ClassCreationDTO dto);
    Task<bool> UpdateAsync(ClassUpdateDTO dto);
    Task<IEnumerable<LessonAttendanceDTO>> GetClassAttendanceByUsernameAsync(SearchClassAttendanceDTO dto);
    Task<IEnumerable<UserAttendanceDTO>> GetClassAttendanceAsync(string id);
}