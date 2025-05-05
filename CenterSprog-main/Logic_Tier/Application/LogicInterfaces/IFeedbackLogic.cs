using Domain.DTOs.FeedbackDTO;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IFeedbackLogic
{
    Task<Feedback> AddFeedbackAsync(AddFeedbackDTO addFeedbackDto);
    Task<Feedback> GetFeedbackByHandInIdAndStudentUsernameAsync(string handInId, string studentUsername);
}