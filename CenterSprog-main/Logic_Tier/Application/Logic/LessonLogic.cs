using System.Data.SqlTypes;
using Application.ClientInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.LessonDTO;
using Domain.Models;
using Google.Protobuf.Reflection;

namespace Application.Logic;

/**
* Class: LessonLogic
* Purpose: Class used to handle the logic of the lesson
* Methods:
*   GetByIdAsync(string id) -> Task<Lesson>
*   MarkAttendanceAsync(MarkAttendanceDTO markAttendanceDto) -> Task<int>
*   GetAttendanceAsync(string id) -> Task<IEnumerable<User>>
*   CreateAsync(LessonCreationDTO lessonCreationDto) -> Task<Lesson>
*   DeleteAsync(string lessonId) -> Task<Boolean>
*   UpdateLessonAsync(LessonUpdateDTO lessonUpdateDto) -> Task<Boolean>
*   ValidateLessonCreationAndUpdate(string topic, string description, long date, string id) -> void
*/

public class LessonLogic : ILessonLogic
{
    private readonly ILessonClient _lessonClient;

    /**
    * 1-arg constructor containing ILessonClient
    * Purpose: Used for client injection
    * @param ILessonClient lessonClient
    */

    public LessonLogic(ILessonClient lessonClient)
    {
        _lessonClient = lessonClient;
    }

    /**
    * Purpose: Method used to get a lesson by id
    * @param string id -> Id of the lesson
    * @return Task<Lesson> -> Lesson object
    */

    public async Task<Lesson> GetByIdAsync(string id)
    {
        return await _lessonClient.GetByIdAsync(id);
    }

    /**
    * Purpose: Method used to mark attendance
    * @param MarkAttendanceDTO markAttendanceDto -> DTO used to mark attendance
    * @return Task<int> -> int value
    */


    public async Task<int> MarkAttendanceAsync(MarkAttendanceDTO markAttendanceDto)
    {
        return await _lessonClient.MarkAttendanceAsync(markAttendanceDto);
    }

    /**
    * Purpose: Method used to get attendance
    * @param string id -> Id of the lesson
    * @return Task<IEnumerable<User>> -> List of User objects
    */

    public async Task<IEnumerable<User>> GetAttendanceAsync(string id)
    {
        return await _lessonClient.GetAttendanceAsync(id);
    }


    /**
    * Purpose: Method used to create a lesson
    * @param LessonCreationDTO lessonCreationDto -> DTO used to create a lesson
    * @return Task<Lesson> -> Lesson object
    */
    public async Task<Lesson> CreateAsync(LessonCreationDTO lessonCreationDto)
    {
        ValidateLessonCreationAndUpdate(lessonCreationDto.Topic, lessonCreationDto.Description, lessonCreationDto.Date,
            lessonCreationDto.ClassId);
        var createdLesson = await _lessonClient.CreateAsync(lessonCreationDto);

        return createdLesson;
    }

    /**
    * Purpose: Method used to delete a lesson
    * @param string lessonId -> Id of the lesson
    * @return Task<Boolean> -> Boolean value
    */
    public async Task<Boolean> DeleteAsync(string lessonId)
    {
        return await _lessonClient.DeleteAsync(lessonId);
    }

    /**
    * Purpose: Method used to update a lesson
    * @param LessonUpdateDTO lessonUpdateDto -> DTO used to update a lesson
    * @return Task<Boolean> -> Boolean value
    */
    public async Task<Boolean> UpdateLessonAsync(LessonUpdateDTO lessonUpdateDto)
    {
        ValidateLessonCreationAndUpdate(lessonUpdateDto.Topic, lessonUpdateDto.Description, lessonUpdateDto.Date,
            lessonUpdateDto.Id);
        return await _lessonClient.UpdateLessonAsync(lessonUpdateDto);
    }


    /**
    * Purpose: Method used to validate the creation and update of a lesson
    * Checks if the lesson id, topic, description and date are valid (not null or empty) and if the topic is at least 3 characters long and the description is at least 10 characters long
    * @param string topic -> Topic of the lesson
    * @param string description -> Description of the lesson
    * @param long date -> Date of the lesson
    * @param string id -> Id of the lesson
    * @throws Exception -> Exception if the validation fails
    * @return void
    */

    public void ValidateLessonCreationAndUpdate(string topic, string description, long date, string id)
    {
        if (string.IsNullOrWhiteSpace(id))
            throw new Exception("Id is required.");
        if (string.IsNullOrWhiteSpace(topic) || topic.Length < 3)
            throw new Exception("Topic must be at least 3 characters long.");
        if (string.IsNullOrWhiteSpace(description) || description.Length < 10)
            throw new Exception("Description must be at least 10 characters long.");
        if (date == 0)
            throw new Exception("Date is required.");
    }
}