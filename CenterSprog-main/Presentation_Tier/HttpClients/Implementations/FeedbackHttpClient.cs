using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs.FeedbackDTO;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class FeedbackHttpClient : IFeedbackService
{
    private readonly HttpClient _client;

    public FeedbackHttpClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Feedback> AddFeedbackAsync(string jwt, AddFeedbackDTO addFeedbackDto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage responseMessage = await _client.PostAsJsonAsync("/feedbacks", addFeedbackDto);
        string responseBody = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }
        Feedback feedback = JsonSerializer.Deserialize<Feedback>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return feedback;
    }

    public async Task<Feedback> GetFeedbackByHandInIdAndStudentUsernameAsync(string jwt, string handInId, string studentUsername)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage responseMessage = await _client.GetAsync($"/handIns/{handInId}/feedback?username={studentUsername}");

        string responseBody = await responseMessage.Content.ReadAsStringAsync();

        if (!responseMessage.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }
        Feedback feedback = JsonSerializer.Deserialize<Feedback>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return feedback;
    }
}