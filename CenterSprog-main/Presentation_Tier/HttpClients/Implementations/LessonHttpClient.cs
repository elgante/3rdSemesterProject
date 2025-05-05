using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs.LessonDTO;
using Domain.Models;
using HttpClients.ClientInterfaces;
using Microsoft.Extensions.Logging;

namespace HttpClients.Implementations;

public class LessonHttpClient : ILessonService
{
    private readonly HttpClient _client;
    public LessonHttpClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Lesson> GetByIdAsync(string jwt, string id)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.GetAsync($"/lessons/{id}");
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }

        Lesson foundLesson = JsonSerializer.Deserialize<Lesson>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;

        return foundLesson;
    }

    public async Task<string> MarkAttendanceAsync(string jwt, MarkAttendanceDTO markAttendanceDto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.PostAsJsonAsync($"/lessons/{markAttendanceDto.LessonId}/attendance",
            markAttendanceDto.StudentUsernames);
        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }

        return responseContent;
    }

    public async Task<IEnumerable<User>> GetAttendanceAsync(string jwt, string id)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.GetAsync($"/lessons/{id}/attendance");

        string responseContent = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }

        ICollection<User> attendees = JsonSerializer.Deserialize<ICollection<User>>(responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;

        return attendees;
    }


    public async Task<Lesson> CreateAsync(string jwt, LessonCreationDTO lessonCreationDto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.PostAsJsonAsync("/lessons", lessonCreationDto);
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        Lesson lesson = JsonSerializer.Deserialize<Lesson>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return lesson;
    }

    public async Task UpdateLessonAsync(string jwt, LessonUpdateDTO lessonUpdateDto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.PutAsJsonAsync("/lessons", lessonUpdateDto);
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }
    }

    public async Task DeleteAsync(string jwt, string lessonId)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.DeleteAsync($"lessons/{lessonId}");
        string responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseContent);
        }
    }
}