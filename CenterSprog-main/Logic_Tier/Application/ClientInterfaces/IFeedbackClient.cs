using Domain.DTOs.FeedbackDTO;
using Domain.Models;

namespace Application.ClientInterfaces;

public interface IFeedbackClient
{
    Task<Feedback> AddFeedbackAsync(AddFeedbackDTO addFeedbackDto);
    Task<Feedback> GetFeedbackByHandInIdAndStudentUsernameAsync(string handInId, string studentUsername);
}