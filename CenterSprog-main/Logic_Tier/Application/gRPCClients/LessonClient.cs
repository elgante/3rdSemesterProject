using Application.ClientInterfaces;
using Domain.DTOs.LessonDTO;
using Grpc.Net.Client;
using gRPCClient;
using Domain.Models;
using Grpc.Core;
using Homework = Domain.Models.Homework;


namespace Application.gRPCClients;

public class LessonClient : ILessonClient
{
    public async Task<Lesson?> GetByIdAsync(string id)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new LessonService.LessonServiceClient(channel);

        var request = new RequestGetLessonById()
        {
            LessonId = id
        };

        var reply = new ResponseGetLessonById();

        reply = await client.getLessonByIdAsync(request);

        Lesson foundLesson = new(reply.Lesson.Id, reply.Lesson.Date, reply.Lesson.Description, reply.Lesson.Topic);

        if (reply.Lesson.Homework != null)
            foundLesson.Homework = new Homework(reply.Lesson.Homework.Id, reply.Lesson.Homework.Deadline,
                reply.Lesson.Homework.Title, reply.Lesson.Homework.Description);

        return await Task.FromResult(foundLesson);
    }

    public async Task<int> MarkAttendanceAsync(MarkAttendanceDTO markAttendanceDto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new LessonService.LessonServiceClient(channel);

        var request = new RequestMarkAttendance()
        {
            LessonId = markAttendanceDto.LessonId,
            Usernames = { markAttendanceDto.StudentUsernames }
        };
        var reply = new ResponseMarkAttendance();
        try
        {
            reply = await client.markAttendanceAsync(request);
        }
        catch (RpcException e)
        {
            Console.WriteLine($" gRPC call failed: {e.Status}");
            throw;
        }

        return await Task.FromResult(reply.AmountOfParticipants);
    }

    public async Task<IEnumerable<User>> GetAttendanceAsync(string id)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new LessonService.LessonServiceClient(channel);

        var request = new RequestGetAttendance()
        {
            LessonId = id
        };

        var reply = new ResponseGetAttendance();

        reply = await client.getAttendanceAsync(request);

        var participants = new List<User>();

        foreach (var participant in reply.Participants)
        {
            participants.Add(new User
            {
                FirstName = participant.FirstName,
                LastName = participant.LastName,
                Username = participant.Username
            });
        }

        return await Task.FromResult(participants);
    }

    public async Task<Lesson> CreateAsync(LessonCreationDTO lessonCreationDto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new LessonService.LessonServiceClient(channel);

        var request = new RequestCreateLesson

        {
            ClassId = lessonCreationDto.ClassId,
            Lesson = new LessonData
            {
                Topic = lessonCreationDto.Topic,
                Date = lessonCreationDto.Date,
                Description = lessonCreationDto.Description,
                Homework = null,
                Id = "",
            }
        };

        var reply = new ResponseCreateLesson();
        try
        {
            reply = await client.CreateLessonAsync(request);
        }
        catch (Exception e)
        {
            Console.WriteLine("HERE: " + e);
        }

        Lesson createdLesson =
            new Lesson(reply.Lesson.Id, reply.Lesson.Date, reply.Lesson.Description, reply.Lesson.Topic);

        return await Task.FromResult(createdLesson);
    }

    public async Task<Boolean> DeleteAsync(string lessonId)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new LessonService.LessonServiceClient(channel);


        var request = new RequestDeleteLesson
        {
            LessonId = lessonId
        };

        var response = client.deleteLesson(request);

        if (response.Status == ResponseDeleteLesson.Types.Status.Ok)
        {
            return true;
        }

        return false;
    }

    public async Task<Boolean> UpdateLessonAsync(LessonUpdateDTO lessonUpdateDto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new LessonService.LessonServiceClient(channel);

        var request = new RequestUpdateLesson
        {
            Id = lessonUpdateDto.Id,
            Lesson = new LessonData
            {
                Topic = lessonUpdateDto.Topic,
                Date = lessonUpdateDto.Date,
                Description = lessonUpdateDto.Description
            }
        };

        var reply = await client.updateLessonAsync(request);
        
        if (reply.Status == ResponseUpdateLesson.Types.Status.Ok)
        {
            return true;
        }

        return false;
    }
}