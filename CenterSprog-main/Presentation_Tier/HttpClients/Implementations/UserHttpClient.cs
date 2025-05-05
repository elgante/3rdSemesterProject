using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Domain.DTOs.UserDTO;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class UserHttpClient : IUserService
{
    private readonly HttpClient _client;

    public UserHttpClient(HttpClient client)
    {
        _client = client;
    }

    public static string? Jwt { get; private set; } = "";
    public Task<ClaimsPrincipal> GetAuthAsync()
    {
        ClaimsPrincipal principal = CreateClaimsPrincipal();
        return Task.FromResult(principal);
    }
    
    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } = null!;

    // Below methods stolen from https://github.com/SteveSandersonMS/presentation-2019-06-NDCOslo/blob/master/demos/MissionControl/MissionControl.Client/Util/ServiceExtensions.cs
    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }

    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
    
    private static ClaimsPrincipal CreateClaimsPrincipal()
    {
        if (string.IsNullOrEmpty(Jwt))
        {
            return new ClaimsPrincipal();
        }

        IEnumerable<Claim> claims = ParseClaimsFromJwt(Jwt);
    
        ClaimsIdentity identity = new(claims, "jwt");

        ClaimsPrincipal principal = new(identity);
        return principal;
    }
    
    public async Task LoginAsync(string username, string password)
    {
        UserLoginDTO userLoginDto = new()
        {
            Username = username,
            Password = password
        };

        string userAsJson = JsonSerializer.Serialize(userLoginDto);
        StringContent content = new(userAsJson, Encoding.UTF8, "application/json");

        HttpResponseMessage response = await _client.PostAsync("/auth/login", content);
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }

        string token = responseContent;
        Jwt = token;
        

        ClaimsPrincipal principal = CreateClaimsPrincipal();
        OnAuthStateChanged.Invoke(principal);
    }

    public Task LogoutAsync()
    {
        Jwt = null;
        ClaimsPrincipal principal = new();
        OnAuthStateChanged.Invoke(principal);
        return Task.CompletedTask;
    }
    
    public async Task<User> CreateUserAsync(UserCreationDTO dto, string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.PostAsJsonAsync("/users", dto);
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        User user = JsonSerializer.Deserialize<User>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return user;
    }

    public async Task<User> GetAsync(string username,string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.GetAsync($"/users/{username}");
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        User user = JsonSerializer.Deserialize<User>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return user;
    }

    public async Task<IEnumerable<User>> GetAllAsync(string token)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        HttpResponseMessage response = await _client.GetAsync($"/users");
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        ICollection<User> users = JsonSerializer.Deserialize<ICollection<User>>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return users;

    }
}