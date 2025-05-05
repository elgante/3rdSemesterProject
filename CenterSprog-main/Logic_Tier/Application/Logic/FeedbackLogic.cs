using Application.ClientInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs.FeedbackDTO;
using Domain.Models;

namespace Application.Logic;

/**
* Class: FeedbackLogic
* Purpose: Class used to handle the logic of the feedback
* Methods:
*   AddFeedbackAsync(AddFeedbackDTO addFeedbackDto) -> Task<Feedback>
*   GetFeedbackByHandInIdAndStudentUsernameAsync(string handInId, string studentUsername) -> Task<Feedback>
*   ValidateFeedbackCreation(AddFeedbackDTO addFeedbackDto) -> void
*/

public class FeedbackLogic : IFeedbackLogic
{
    private readonly IFeedbackClient _feedbackClient;

    /**
    * 1-arg constructor containing IFeedbackClient
    * Purpose: Used for client injection
    * @param IFeedbackClient feedbackClient
    */

    public FeedbackLogic(IFeedbackClient feedbackClient)
    {
        _feedbackClient = feedbackClient;
    }

    /**
    * Purpose: Method used to add a feedback
    * @param AddFeedbackDTO addFeedbackDto -> DTO used to add a feedback
    * @return Task<Feedback> -> Feedback object
    */
    public async Task<Feedback> AddFeedbackAsync(AddFeedbackDTO addFeedbackDto)
    {
        ValidateFeedbackCreation(addFeedbackDto);
        return await _feedbackClient.AddFeedbackAsync(addFeedbackDto);
    }

    /**
    * Purpose: Method used to get a feedback by hand in id and student username
    * @param string handInId -> Id of the hand in
    * @param string studentUsername -> Username of the student
    * @return Task<Feedback> -> Feedback object
    */
    public async Task<Feedback> GetFeedbackByHandInIdAndStudentUsernameAsync(string handInId, string studentUsername)
    {
        return await _feedbackClient.GetFeedbackByHandInIdAndStudentUsernameAsync(handInId, studentUsername);
    }

    /**
    * Purpose: Method used to validate the creation of a feedback. 
    * Checks if the student username, hand in id, grade and comment are valid (not null or empty) and if the grade is one of the allowed grades (-3, 0, 2, 4, 7, 10, 12)
    * @param AddFeedbackDTO addFeedbackDto -> DTO used to add a feedback
    * @throws Exception - if the student username is null or empty
    * @return void
    */

    public void ValidateFeedbackCreation(AddFeedbackDTO addFeedbackDto)
    {
        int[] allowedGrades = { -3, 0, 2, 4, 7, 10, 12 };

        if (string.IsNullOrEmpty(addFeedbackDto.StudentUsername))
            throw new Exception("Student Username is required");
        if (string.IsNullOrEmpty(addFeedbackDto.HandInId))
            throw new Exception("HandIn Id is required");
        if (!allowedGrades.Contains(addFeedbackDto.Grade))
            throw new Exception("Grade must be one of these numbers: -3, 0, 2, 4, 7, 10, 12");
        if (string.IsNullOrWhiteSpace(addFeedbackDto.Comment) || addFeedbackDto.Comment.Length < 5)
            throw new Exception("Comment must be at least 5 characters long.");
    }
}