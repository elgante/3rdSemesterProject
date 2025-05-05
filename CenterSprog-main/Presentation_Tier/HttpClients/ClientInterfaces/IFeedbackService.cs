using Domain.DTOs.FeedbackDTO;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IFeedbackService
{
    Task<Feedback> AddFeedbackAsync(string jwt, AddFeedbackDTO addFeedbackDto);
    Task<Feedback> GetFeedbackByHandInIdAndStudentUsernameAsync(string jwt, string handInId, string studentUsername);
}