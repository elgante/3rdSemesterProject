using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using Domain.DTOs.HomeworkDTO;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class HomeworkHttpClient : IHomeworkService
{
    private readonly HttpClient _client;

    public HomeworkHttpClient(HttpClient client)
    {
        _client = client;
    }

    public async Task<Homework> CreateAsync(string jwt, HomeworkCreationDTO dto)
    {
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
        HttpResponseMessage response = await _client.PostAsJsonAsync("/homeworks", dto);
        string responseBody = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(responseBody);
        }

        Homework homework = JsonSerializer.Deserialize<Homework>(responseBody,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            })!;
        return homework;
    }
}