using Application.ClientInterfaces;
using Domain.DTOs.HomeworkDTO;
using Grpc.Net.Client;
using gRPCClient;
using Homework = Domain.Models.Homework;

namespace Application.gRPCClients;

public class HomeworkClient : IHomeworkClient
{
    public async Task<Homework> CreateAsync(HomeworkCreationDTO dto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new HomeworkService.HomeworkServiceClient(channel);
        
        var request = new RequestAddHomework
        {
            LessonId = dto.LessonId,
            Homework = new gRPCClient.Homework
            {
                Deadline = dto.Deadline,
                Description = dto.Description,
                Title = dto.Title
            }
        };
        var reply = new gRPCClient.Homework();
        try
        {
            reply = await client.addHomeworkAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        Homework createdHomework = new Homework(reply.Id, reply.Deadline, reply.Title, reply.Description);
        return await Task.FromResult(createdHomework);
    }
}