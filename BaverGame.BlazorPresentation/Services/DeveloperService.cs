using BaverGame.BlazorPresentation.Models;

namespace BaverGame.BlazorPresentation.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class DeveloperService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://localhost:8001";

    public DeveloperService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get all developers
    public async Task<List<Developer>> GetDevelopersAsync()
    {
        var items = await _httpClient.GetAsync(ApiUrl + "/api/DeveloperApi");
        return (await items.Content.ReadFromJsonAsync<List<Developer>>())!;
    }

    // Get a single developer by ID
    public async Task<Developer> GetDeveloperByIdAsync(Guid id)
    {
        var item = await _httpClient.GetAsync(ApiUrl + $"/api/DeveloperApi/{id}");
        return (await item.Content.ReadFromJsonAsync<Developer>())!;
    }

    // Add a new developer
    public async Task<Developer> AddDeveloperAsync(Developer developer)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiUrl + $"/api/DeveloperApi", developer);
        response.EnsureSuccessStatusCode();

        // Deserialize and return the new developer
        return (await response.Content.ReadFromJsonAsync<Developer>())!;
    }

    // Update an existing developer
    public async Task UpdateDeveloperAsync(Guid id, Developer developer)
    {
        var response = await _httpClient.PutAsJsonAsync(ApiUrl + $"/api/DeveloperApi/{id}", developer);
        response.EnsureSuccessStatusCode();
    }

    // Delete a developer
    public async Task DeleteDeveloperAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync(ApiUrl+ $"/api/DeveloperApi/{id.ToString()}");
        response.EnsureSuccessStatusCode();
    }
}
