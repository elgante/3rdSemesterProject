using Application.ClientInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.ClassDTO;
using Domain.DTOs.LessonDTO;
using Domain.DTOs.UserDTO;
using Domain.Models;

namespace Application.Logic;


/**
* Class: ClassLogic
* Purpose: Class used to handle the logic of the class
* Methods:
*   GetByIdAsync(string id) -> Task<ClassEntity>
*   GetAllAsync(SearchClassDTO dto) -> Task<IEnumerable<ClassEntity>>
*   GetAllParticipantsAsync(SearchClassParticipantsDTO dto) -> Task<IEnumerable<User>>
*   CreateAsync(ClassCreationDTO dto) -> Task<ClassEntity>
*   UpdateAsync(ClassUpdateDTO dto) -> Task<bool>
*   GetClassAttendanceByUsernameAsync(SearchClassAttendanceDTO dto) -> Task<IEnumerable<LessonAttendanceDTO>>
*   GetClassAttendanceAsync(string id) -> Task<IEnumerable<UserAttendanceDTO>>
*   ValidateClassCreation(ClassCreationDTO dto) -> void
*/

public class ClassLogic : IClassLogic
{
    private readonly IClassClient _classClient;

    /**
    * 1-arg constructor containing IClassClient
    * Purpose: Used for client injection
    * @param IClassClient classClient
    */

    public ClassLogic(IClassClient classClient)
    {
        _classClient = classClient;
    }

    /**
    * Purpose: Method used to get a class by id
    *  @param string id -> Id of the class
    *  @return Task<ClassEntity> -> ClassEntity object
    */

    public async Task<ClassEntity> GetByIdAsync(string id)
    {
        return await _classClient.GetByIdAsync(id);
    }

    /**
    * Purpose: Method used to get all classes
    * @param SearchClassDTO dto -> DTO used to search for classes
    * @return Task<IEnumerable<ClassEntity>> -> List of ClassEntity objects
    */

    public async Task<IEnumerable<ClassEntity>> GetAllAsync(SearchClassDTO dto)
    {
        return await _classClient.GetAllAsync(dto);
    }

    /**
    * Purpose: Method used to get all participants of a class
    * @param SearchClassParticipantsDTO dto -> DTO used to search for participants
    * @return Task<IEnumerable<User>> -> List of User objects
    */

    public async Task<IEnumerable<User>> GetAllParticipantsAsync(SearchClassParticipantsDTO dto)
    {
        // Validate the role
        return await _classClient.GetAllParticipantsAsync(dto);
    }

    /**
    * Purpose: Method used to create a class
    * @param ClassCreationDTO dto -> DTO used to create a class
    * @return Task<ClassEntity> -> ClassEntity object
    */

    public async Task<ClassEntity> CreateAsync(ClassCreationDTO dto)
    {
        ValidateClassCreation(dto);
        ClassEntity createdClass = await _classClient.CreateAsync(dto);
        return await Task.FromResult(createdClass);
    }

    /**
    * Purpose: Method used to update a class
    * @param ClassUpdateDTO dto -> DTO used to update a class
    * @return Task<bool> -> Boolean value
    */
    public async Task<bool> UpdateAsync(ClassUpdateDTO dto)
    {
        // Here in the logic in the future you may want to update the class based on other params like title or room
        if (dto.Participants != null)
        {
            bool result = await _classClient.UpdateParticipants(dto);
            return await Task.FromResult(result);
        }

        return false;
    }

    /**
    * Purpose: Method used to get the attendance of a class by username
    * @param SearchClassAttendanceDTO dto -> DTO used to search for the attendance of a class
    * @return Task<IEnumerable<LessonAttendanceDTO>> -> List of LessonAttendanceDTO objects
    */

    public async Task<IEnumerable<LessonAttendanceDTO>> GetClassAttendanceByUsernameAsync(SearchClassAttendanceDTO dto)
    {
        var lessonsAttended = await _classClient.GetClassAttendanceByUsernameAsync(dto);
        var lessons = (await _classClient.GetByIdAsync(dto.Id)).Lessons;
        var lessonAttendance = lessons
            .Select(lesson => new LessonAttendanceDTO
            {
                Topic = lesson.Topic,
                Date = lesson.Date,
                HasAttended = lessonsAttended.Any(attendedLesson => attendedLesson.Id == lesson.Id)
            });
        return lessonAttendance;
    }

    /**
    * Purpose: Method used to get the attendance of a class
    * @param string id -> Id of the class
    * @return Task<IEnumerable<UserAttendanceDTO>> -> List of UserAttendanceDTO objects
    */

    public async Task<IEnumerable<UserAttendanceDTO>> GetClassAttendanceAsync(string id)
    {
        var participants = await _classClient.GetAllParticipantsAsync(new SearchClassParticipantsDTO { Id = id, Role = "student" });
        var lessonAttendance = await _classClient.GetClassAttendanceAsync(id);
        var lessons = (await _classClient.GetByIdAsync(id)).Lessons;
        var participantsWithAttendance = new List<UserAttendanceDTO>();

        foreach (var participant in participants)
        {
            var userAttendanceDto = new UserAttendanceDTO
            {
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                Email = participant.Email
            };

            var totalLessons = lessons.Count;
            var attendedLessons = lessonAttendance
                .Count(lesson => lesson.Attendees.Any(attendance => attendance.Username == participant.Username));

            var lessonsMissed = totalLessons - attendedLessons;
            var totalAbsence = totalLessons == 0 ? 0 : (double)lessonsMissed / totalLessons * 100;

            userAttendanceDto.TotalAbsence = totalAbsence;

            participantsWithAttendance.Add(userAttendanceDto);
        }

        return participantsWithAttendance;
    }

    /**
    * Purpose: Method used to validate the creation of a class
    * Validation:
    *   - Title is required
    *   - Room is required
    * @param ClassCreationDTO dto -> DTO used to create a class
    * @throws Exception -> If the title or room is null or empty
    * @return void
    */

    public void ValidateClassCreation(ClassCreationDTO dto)
    {
        if (string.IsNullOrEmpty(dto.Title))
            throw new Exception("Title is required");
        if (string.IsNullOrEmpty(dto.Room))
            throw new Exception("Room is required");
    }
}