using Application.ClientInterfaces;
using Domain.DTOs.ClassDTO;
using Domain.Models;
using Google.Protobuf.Collections;
using Grpc.Core;
using Grpc.Net.Client;
using gRPCClient;
using ClassEntity = Domain.Models.ClassEntity;

namespace Application.gRPCClients;

public class ClassClient : IClassClient
{
    public async Task<ClassEntity> GetByIdAsync(string id)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new ClassEntityService.ClassEntityServiceClient(channel);

        var request = new RequestGetClassEntity
        {
            ClassId = id
        };
        
        var reply = await client.getClassEntityByIdAsync(request);

        ClassEntity retrievedClassEntity = new(reply.ClassEntity.Id, reply.ClassEntity.Title, reply.ClassEntity.Room);

        if (reply.ClassEntity.Lessons.Any())
        {
            foreach (var lesson in reply.ClassEntity.Lessons)
            {
                retrievedClassEntity.Lessons.Add(new Lesson(lesson.Id, lesson.Date, lesson.Description, lesson.Topic));
            }
        }

        return await Task.FromResult(retrievedClassEntity);
    }

    public async Task<IEnumerable<ClassEntity>> GetAllAsync(SearchClassDTO dto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new ClassEntityService.ClassEntityServiceClient(channel);
        var request = new RequestGetClassEntities();
        if (dto.Username != null)
            request.Username = dto.Username;

        var reply = await client.getClassEntitiesAsync(request);

        var classes = new List<ClassEntity>();

        foreach (var classData in reply.ClassEntities)
        {
            ClassEntity newClass = new ClassEntity(classData.Id, classData.Title, classData.Room);

            foreach (var userParticipant in classData.Participants)
            {
                newClass.Participants.Add(new User(userParticipant.Username, userParticipant.FirstName,
                    userParticipant.LastName, userParticipant.Role));
            }

            classes.Add(newClass);
        }

        return await Task.FromResult(classes);
    }

    public async Task<IEnumerable<User>> GetAllParticipantsAsync(SearchClassParticipantsDTO dto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new ClassEntityService.ClassEntityServiceClient(channel);
        var request = new RequestGetClassParticipants
        {
            ClassId = dto.Id
        };
        if (dto.Role != null)
            request.Role = dto.Role;

        var reply = await client.getClassParticipantsAsync(request);

        var participants = new List<User>();

        foreach (var participant in reply.Participants)
        {
            participants.Add(new User(participant.Username, participant.FirstName, participant.LastName,
                participant.Role, participant.Email));
        }

        return await Task.FromResult(participants);
    }

    public async Task<ClassEntity> CreateAsync(ClassCreationDTO dto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new ClassEntityService.ClassEntityServiceClient(channel);
        var request = new RequestCreateClassEntity
        {
            ClassEntityCreation = new ClassEntityCreation
            {
                Room = dto.Room,
                Title = dto.Title
            }
        };
        var reply = await client.createClassEntityAsync(request);

        ClassEntity createdClass =
            new ClassEntity(reply.ClassEntity.Id, reply.ClassEntity.Title, reply.ClassEntity.Room);

        return await Task.FromResult(createdClass);
    }

    public async Task<bool> UpdateParticipants(ClassUpdateDTO dto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new ClassEntityService.ClassEntityServiceClient(channel);
        var participantsData = new RepeatedField<string>();
        foreach (string username in dto.Participants)
        {
            participantsData.Add(username);
        }

        var request = new RequestUpdateClassParticipants
        {
            Id = dto.Id,
            ParticipantsUsernames = { participantsData }
        };

        var reply = await client.updateParticipantsAsync(request);

        return await Task.FromResult(reply.Result);
    }

    public async Task<IEnumerable<Lesson>> GetClassAttendanceByUsernameAsync(SearchClassAttendanceDTO dto)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new ClassEntityService.ClassEntityServiceClient(channel);

        var request = new RequestGetClassAttendanceByUsername
        {
            ClassId = dto.Id,
            Username = dto.Username
        };

        var reply = await client.getClassAttendanceByUsernameAsync(request);

        var lessons = new List<Lesson>();

        if (reply.Lessons.Any())
        {
            foreach (var lesson in reply.Lessons)
            {
                lessons.Add(new Lesson { Id = lesson.Id, Topic = lesson.Topic, Date = lesson.Date });
            }
        }

        return await Task.FromResult(lessons);
    }

    public async Task<IEnumerable<Lesson>> GetClassAttendanceAsync(string id)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new ClassEntityService.ClassEntityServiceClient(channel);

        var request = new RequestGetClassAttendance
        {
            ClassId = id
        };

        var reply = new ResponseGetClassAttendance();

        reply = await client.getClassAttendanceAsync(request);

        var lessons = new List<Lesson>();

        if (reply.LessonsAttendance.Any())
        {
            foreach (var lesson in reply.LessonsAttendance)
            {
                var participants = new List<User>();
                foreach (var userParticipant in lesson.Participants)
                {
                    participants.Add(new User { Username = userParticipant.Username });
                }

                lessons.Add(new Lesson { Id = lesson.Id, Attendees = participants });
            }
        }

        return await Task.FromResult(lessons);
    }
}