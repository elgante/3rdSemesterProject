using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs.ClassDTO;
using Domain.DTOs.LessonDTO;
using Domain.DTOs.UserDTO;
using Domain.Models;
using HttpClients.ClientInterfaces;


namespace HttpClients.Implementations;

public class ClassHttpClient : IClassService
{
    private readonly HttpClient _client;

    public ClassHttpClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<ClassEntity> GetByIdAsync(string jwt, string id)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage responseMessage = await _client.GetAsync($"/classes/{id}");

        string responseBody = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        ClassEntity classEntity = JsonSerializer.Deserialize<ClassEntity>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return classEntity;
    }

    public async Task<IEnumerable<ClassEntity>> GetAllAsync(string jwt, SearchClassDTO dto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        String username = dto.Username;
        String url = "/classes";
        if (username != null)
        {
            url += $"?username={username}";
        }

        HttpResponseMessage responseMessage = await _client.GetAsync(url);
        string responseBody = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(
                responseBody);
        }

        ICollection<ClassEntity> classes = JsonSerializer.Deserialize<ICollection<ClassEntity>>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return classes;
    }

    public async Task<IEnumerable<User>> GetAllParticipantsAsync(string jwt, SearchClassParticipantsDTO dto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        string query = "";
        if (!string.IsNullOrEmpty(dto.Role))
            query += $"?role={dto.Role}";
        HttpResponseMessage responseMessage = await _client.GetAsync($"/classes/{dto.Id}/participants" + query);

        string responseBody = await responseMessage.Content.ReadAsStringAsync();
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        ICollection<User> participants = JsonSerializer.Deserialize<ICollection<User>>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return participants;
    }

    public async Task<ClassEntity> CreateAsync(string jwt, ClassCreationDTO dto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.PostAsJsonAsync("/classes/", dto);
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        ClassEntity classEntity = JsonSerializer.Deserialize<ClassEntity>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return classEntity;
    }

    public async Task<bool> UpdateClass(string jwt, ClassUpdateDTO dto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.PatchAsJsonAsync($"/classes/{dto.Id}", dto);
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        bool updated = JsonSerializer.Deserialize<Boolean>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return updated;
    }

    public async Task<IEnumerable<UserAttendanceDTO>> GetClassAttendanceAsync(string jwt, string id)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage responseMessage = await _client.GetAsync($"/classes/{id}/attendances");

        string responseBody = await responseMessage.Content.ReadAsStringAsync();
        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        ICollection<UserAttendanceDTO> attendees = JsonSerializer.Deserialize<ICollection<UserAttendanceDTO>>(
            responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return attendees;
    }

    public async Task<IEnumerable<LessonAttendanceDTO>> GetClassAttendanceByUsernameAsync(string jwt, SearchClassAttendanceDTO dto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage responseMessage =
            await _client.GetAsync($"/classes/{dto.Id}/attendances?username={dto.Username}");
        string responseBody = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        ICollection<LessonAttendanceDTO> lessons = JsonSerializer.Deserialize<ICollection<LessonAttendanceDTO>>(
            responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return lessons;
    }
}