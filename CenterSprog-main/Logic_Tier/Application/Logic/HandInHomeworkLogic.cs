using Application.ClientInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.HomeworkDTO;
using Domain.Models;

namespace Application.Logic;

/**
* Class: HandInHomeworkLogic
* Purpose: Class used to handle the logic of the hand in homework
* Methods:
*   HandInHomework(HomeworkHandInDTO dto) -> Task<HandInHomework>
*   GetHandInsByHomeworkIdAsync(string homeworkId) -> Task<IEnumerable<HandInHomework>>
*   GetHandInByHomeworkIdAndStudentUsernameAsync(string homeworkId, string studentUsername) -> Task<HandInHomework>
*   ValidateHandInCreation(HomeworkHandInDTO dto) -> void
*/

public class HandInHomeworkLogic : IHandInHomeworkLogic
{
    private readonly IHandInHomeworkClient _handInHomeworkClient;

    /**
    * 1-arg constructor containing IHandInHomeworkClient
    * Purpose: Used for client injection
    * @param IHandInHomeworkClient handInHomeworkClient
    */

    public HandInHomeworkLogic(IHandInHomeworkClient handInHomeworkClient)
    {
        _handInHomeworkClient = handInHomeworkClient;
    }

    /**
    * Purpose: Method used to hand in a homework
    * @param HomeworkHandInDTO dto -> DTO used to hand in a homework
    * @return Task<HandInHomework> -> HandInHomework object
    */
    public async Task<HandInHomework> HandInHomework(HomeworkHandInDTO dto)
    {
        ValidateHandInCreation(dto);
        HandInHomework homeworkToHandIn = await _handInHomeworkClient.HandInHomework(dto);
        return await Task.FromResult(homeworkToHandIn);
    }

    /**
    * Purpose: Method used to get all hand ins by homework id
    * @param string homeworkId -> Id of the homework
    * @return Task<IEnumerable<HandInHomework>> -> List of HandInHomework objects
    */


    public async Task<IEnumerable<HandInHomework>> GetHandInsByHomeworkIdAsync(string homeworkId)
    {
        return await _handInHomeworkClient.GetHandInsByHomeworkIdAsync(homeworkId);
    }

    /**
    * Purpose: Method used to get a hand in by homework id and student username
    * @param string homeworkId -> Id of the homework
    * @param string studentUsername -> Username of the student
    * @return Task<HandInHomework> -> HandInHomework object
    */

    public async Task<HandInHomework> GetHandInByHomeworkIdAndStudentUsernameAsync(string homeworkId,
        string studentUsername)
    {
        return await _handInHomeworkClient.GetHandInByHomeworkIdAndStudentUsernameAsync(homeworkId, studentUsername);
    }

    /**
    * Purpose: Method used to validate the creation of a hand in.
    * Checks if the student username, homework id and answer are valid (not null or empty)
    * @param HomeworkHandInDTO dto -> DTO used to hand in a homework
    * @throws Exception - if the student username is null or empty
    
    * @return void
    */
    public void ValidateHandInCreation(HomeworkHandInDTO dto)
    {
        if (string.IsNullOrEmpty(dto.StudentUsername))
            throw new Exception("Student Username is required");
        if (string.IsNullOrEmpty(dto.HomeworkId))
            throw new Exception("Homework Id is required");
        if (string.IsNullOrWhiteSpace(dto.Answer))
            throw new Exception("Answer is required.");
    }
}