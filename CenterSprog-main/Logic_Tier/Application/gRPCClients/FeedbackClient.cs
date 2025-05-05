using Application.ClientInterfaces;
using Domain.DTOs.FeedbackDTO;
using Grpc.Net.Client;
using gRPCClient;
using Feedback = Domain.Models.Feedback;

namespace Application.gRPCClients;

public class FeedbackClient : IFeedbackClient
{
    public async Task<Feedback> AddFeedbackAsync(AddFeedbackDTO addFeedbackDto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new FeedbackService.FeedbackServiceClient(channel);

        var request = new RequestAddFeedback
        {
            HandInId = addFeedbackDto.HandInId,
            StudentUsername = addFeedbackDto.StudentUsername,
            Feedback = new gRPCClient.Feedback
            {
                Grade = addFeedbackDto.Grade,
                Comment = addFeedbackDto.Comment
            }
        };

        var reply = await client.addFeedbackAsync(request);

        Feedback createdFeedback = new Feedback(reply.Id, reply.Grade, reply.Comment);
        return await Task.FromResult(createdFeedback);
    }

    public async Task<Feedback> GetFeedbackByHandInIdAndStudentUsernameAsync(string handInId, string studentUsername)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new FeedbackService.FeedbackServiceClient(channel);

        var request = new RequestGetFeedbackByHandInIdAndStudentUsername
        {
            HandInId = handInId,
            StudentUsername = studentUsername
        };
        var response = new ResponseGetFeedback();
        try
        {

            Console.WriteLine("HERE LOOKING FOR FEEDBACK");
            response = await client.getFeedbackByHandInIdAndStudentUsernameAsync(request);

            if (response.Feedback == null)
                throw new Exception("No feedback exists yet.");

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message + e.StackTrace);
            throw e;
        }
        
        var feedback = new Feedback(response.Feedback.Id, response.Feedback.Grade, response.Feedback.Comment);
        return await Task.FromResult(feedback);
    }
}