using Application.ClientInterfaces;
using Domain.DTOs.HomeworkDTO;
using Domain.Models;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCClient;
using Feedback = Domain.Models.Feedback;
using HandInHomework = Domain.Models.HandInHomework;

namespace Application.gRPCClients;

public class HandInHomeworkClient : IHandInHomeworkClient
{
    public async Task<HandInHomework> HandInHomework(HomeworkHandInDTO dto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new HandInHomeworkService.HandInHomeworkServiceClient(channel);

        var request = new RequestCreateHandInHomework
        {
            HomeworkId = dto.HomeworkId,
            StudentUsername = dto.StudentUsername,
            Answer = dto.Answer
        };

        var reply = client.handInHomework(request);

        HandInHomework createdHandIn =
            new HandInHomework(reply.HandInHomework.Id, reply.HandInHomework.Answer,
                reply.HandInHomework.StudentUsername);

        return await Task.FromResult(createdHandIn);
    }

    public async Task<IEnumerable<HandInHomework>> GetHandInsByHomeworkIdAsync(string homeworkId)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new HandInHomeworkService.HandInHomeworkServiceClient(channel);

        var request = new RequestGetHandInsByHomeworkId
        {
            HomeworkId = homeworkId
        };

        var reply = client.getHandInsByHomeworkId(request);

        var handIns = new List<HandInHomework>();

        foreach (var grpcHandIn in reply.HandIns)
        {
            var handIn = new HandInHomework(
                grpcHandIn.Id,
                grpcHandIn.Answer,
                grpcHandIn.StudentUsername
            );
            if (grpcHandIn.Feedback != null)
            {
                handIn.Feedback = new Feedback(grpcHandIn.Feedback.Id, grpcHandIn.Feedback.Grade,
                    grpcHandIn.Feedback.Comment);
            }

            handIns.Add(handIn);
        }

        return await Task.FromResult(handIns);
    }

    public async Task<HandInHomework> GetHandInByHomeworkIdAndStudentUsernameAsync(string homeworkId,
        string studentUsername)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new HandInHomeworkService.HandInHomeworkServiceClient(channel);

        var request = new RequestGetHandInByHomeworkIdAndStudentUsername
        {
            HomeworkId = homeworkId,
            StudentUsername = studentUsername
        };

        var reply = new ResponseGetHandInHomework();

        reply = client.getHandInByHomeworkIdAndStudentUsername(request);

        var handIn = new HandInHomework(
            reply.HandInHomework.Id,
            reply.HandInHomework.Answer,
            reply.HandInHomework.StudentUsername
        );
        if (reply.HandInHomework.Feedback != null)
        {
            handIn.Feedback = new Feedback(reply.HandInHomework.Feedback.Id, reply.HandInHomework.Feedback.Grade,
                reply.HandInHomework.Feedback.Comment);
        }

        return await Task.FromResult(handIn);
    }
}