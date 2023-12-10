using BaverGame.BlazorPresentation.Models;

namespace BaverGame.BlazorPresentation.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class PublisherService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://localhost:8001";

    public PublisherService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get all publishers
    public async Task<List<Publisher>> GetPublishersAsync()
    {
        var items = await _httpClient.GetAsync(ApiUrl + "/api/PublisherApi");
        return (await items.Content.ReadFromJsonAsync<List<Publisher>>())!;
    }

    // Get a single publisher by ID
    public async Task<Publisher> GetPublisherByIdAsync(Guid id)
    {
        var item = await _httpClient.GetAsync(ApiUrl + $"/api/PublisherApi/{id}");
        return (await item.Content.ReadFromJsonAsync<Publisher>())!;
    }

    // Add a new publisher
    public async Task<Publisher> AddPublisherAsync(Publisher publisher)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiUrl + $"/api/PublisherApi", publisher);
        response.EnsureSuccessStatusCode();

        // Deserialize and return the new publisher
        return (await response.Content.ReadFromJsonAsync<Publisher>())!;
    }

    // Update an existing publisher
    public async Task UpdatePublisherAsync(Guid id, Publisher publisher)
    {
        var response = await _httpClient.PutAsJsonAsync(ApiUrl + $"/api/PublisherApi/{id}", publisher);
        response.EnsureSuccessStatusCode();
    }

    // Delete a publisher
    public async Task DeletePublisherAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync(ApiUrl + $"/api/PublisherApi/{id}");
        response.EnsureSuccessStatusCode();
    }
}