using Application.ClientInterfaces;
using Application.gRPCClients;
using Application.LogicInterfaces;
using Domain.DTOs.HomeworkDTO;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Application.Logic;
/**
* Class: HomeworkLogic
* Purpose: Class used to handle the logic of the homework
* Methods:
*   CreateAsync(HomeworkCreationDTO dto) -> Task<Homework>
*   ValidateHomeworkCreation(HomeworkCreationDTO dto) -> void
*/
public class HomeworkLogic : IHomeworkLogic
{
    private readonly IHomeworkClient _homeworkClient;

    /**
    * 1-arg constructor containing IHomeworkClient
    * Purpose: Used for client injection
    * @param IHomeworkClient homeworkClient
    */


    public HomeworkLogic(IHomeworkClient homeworkClient)
    {
        _homeworkClient = homeworkClient;
    }

    /**
    * Purpose: Method used to create a homework
    * @param HomeworkCreationDTO dto -> DTO used to create a homework
    * @return Task<Homework> -> Homework object
    */

    public async Task<Homework> CreateAsync(HomeworkCreationDTO dto)
    {
        ValidateHomeworkCreation(dto);
        Homework createdHomework = await _homeworkClient.CreateAsync(dto);
        return await Task.FromResult(createdHomework);
    }


    /**
    * Purpose: Method used to validate the creation of a homework.
    * Checks if the lesson id, title and description are valid (not null or empty) and if the description is at least 10 characters long
    * @param HomeworkCreationDTO dto -> DTO used to create a homework
    * @throws Exception - if the lesson id is null or empty
    * @return void
    */

    public void ValidateHomeworkCreation(HomeworkCreationDTO dto)
    {
        if (string.IsNullOrWhiteSpace(dto.LessonId))
            throw new Exception("Lesson Id is required.");
        if (string.IsNullOrWhiteSpace(dto.Title))
            throw new Exception("Title is required.");
        if (string.IsNullOrWhiteSpace(dto.Description) || dto.Description.Length < 10)
            throw new Exception("Description must be at least 10 characters long");
    }
}