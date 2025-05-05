using Domain.DTOs.ClassDTO;
using Domain.DTOs.LessonDTO;
using Domain.DTOs.UserDTO;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IClassService
{
    Task<ClassEntity> GetByIdAsync(string jwt, string id);
    Task<IEnumerable<ClassEntity>> GetAllAsync(string jwt, SearchClassDTO dto);
    Task<IEnumerable<User>> GetAllParticipantsAsync(string jwt, SearchClassParticipantsDTO dto);
    Task<ClassEntity> CreateAsync(string jwt, ClassCreationDTO dto);
    Task<Boolean> UpdateClass(string jwt, ClassUpdateDTO dto);
    Task<IEnumerable<UserAttendanceDTO>> GetClassAttendanceAsync(string jwt, string id);
    Task<IEnumerable<LessonAttendanceDTO>> GetClassAttendanceByUsernameAsync(string jwt, SearchClassAttendanceDTO dto);
}