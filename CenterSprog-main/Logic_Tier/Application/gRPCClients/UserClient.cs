using Application.ClientInterfaces;
using Domain.DTOs.UserDTO;
using Domain.Models;
using Grpc.Net.Client;
using gRPCClient;

namespace Application.gRPCClients;

public class UserClient : IUserClient
{
    public async Task<User> GetUserByUsernameAsync(string username)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new UserService.UserServiceClient(channel);

        var request = new RequestUserGetByUsername
        {
            Username = username
        };
        var reply = new ResponseUserGetByUsername();

        reply = await client.getUserByUsernameAsync(request);

        var exisingUser = new User(
            reply.User.Username,
            reply.User.Password,
            reply.User.FirstName,
            reply.User.LastName,
            reply.User.Email,
            reply.User.Role
        );
        return await Task.FromResult(exisingUser);
    }

    public async Task<User> CreateUserAsync(UserCreationDTO dto, string username, string password)
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new UserService.UserServiceClient(channel);
        var request = new RequestCreateUser
        {
            User = new UserData
            {
                Email = dto.Email,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Role = dto.Role,
                Username = username,
                Password = password
            }
        };

        var reply =  await client.createUserAsync(request);

        User createdUser = new User(
            reply.User.Username,
            reply.User.Password,
            reply.User.FirstName,
            reply.User.LastName,
            reply.User.Email,
            reply.User.Role
        );
        return await Task.FromResult(createdUser);
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
        using var channel = GrpcChannel.ForAddress("http://localhost:1111");
        var client = new UserService.UserServiceClient(channel);
        var request = new Google.Protobuf.WellKnownTypes.Empty();
        
        var reply = await client.getAllUsersAsync(request);
        
        List<User> retrievedUsers = new List<User>();
        foreach (var userData in reply.Users)
        {
            retrievedUsers.Add(new User(
                userData.Username, userData.Password, userData.FirstName, userData.LastName, userData.Email,
                userData.Role));
        }

        return await Task.FromResult(retrievedUsers);
    }
}