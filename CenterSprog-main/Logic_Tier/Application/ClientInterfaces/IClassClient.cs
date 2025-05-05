using Domain.DTOs.ClassDTO;
using Domain.Models;

namespace Application.ClientInterfaces;

public interface IClassClient
{
    Task<ClassEntity> GetByIdAsync(string id);
    Task<IEnumerable<ClassEntity>> GetAllAsync(SearchClassDTO dto);
    Task<IEnumerable<User>> GetAllParticipantsAsync(SearchClassParticipantsDTO dto);
    Task<ClassEntity> CreateAsync(ClassCreationDTO dto);
    Task<bool> UpdateParticipants(ClassUpdateDTO dto);
    Task<IEnumerable<Lesson>> GetClassAttendanceByUsernameAsync(SearchClassAttendanceDTO dto);
    Task<IEnumerable<Lesson>> GetClassAttendanceAsync(string id);
}