using System.Security.Claims;
using Domain.DTOs.UserDTO;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IUserService
{
    public Task LoginAsync(string username, string password);
    public Task LogoutAsync();
    public Task<ClaimsPrincipal> GetAuthAsync();
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
    public Task<User> CreateUserAsync(UserCreationDTO dto, string token);
    public Task<User> GetAsync(string username, string token);
    public Task<IEnumerable<User>> GetAllAsync(string token);
}