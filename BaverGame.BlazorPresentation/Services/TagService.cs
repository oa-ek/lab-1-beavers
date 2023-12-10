using BaverGame.BlazorPresentation.Models;

namespace BaverGame.BlazorPresentation.Services;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

public class TagService
{
    private readonly HttpClient _httpClient;
    private const string ApiUrl = "https://localhost:8001";

    public TagService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // Get all tags
    public async Task<List<Tag>> GetTagsAsync()
    {
        var response = await _httpClient.GetAsync(ApiUrl + "/api/TagApi");
        return (await response.Content.ReadFromJsonAsync<List<Tag>>())!;
    }

    // Get a single tag by ID
    public async Task<Tag> GetTagByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync(ApiUrl + $"/api/TagApi/{id}");
        return (await response.Content.ReadFromJsonAsync<Tag>())!;
    }

    // Add a new tag
    public async Task<Tag> AddTagAsync(Tag tag)
    {
        var response = await _httpClient.PostAsJsonAsync(ApiUrl + "/api/TagApi", tag);
        response.EnsureSuccessStatusCode();

        // Deserialize and return the new tag
        return (await response.Content.ReadFromJsonAsync<Tag>())!;
    }

    // Update an existing tag
    public async Task UpdateTagAsync(Guid id, Tag tag)
    {
        var response = await _httpClient.PutAsJsonAsync(ApiUrl + $"/api/TagApi/{id}", tag);
        response.EnsureSuccessStatusCode();
    }

    // Delete a tag
    public async Task DeleteTagAsync(Guid id)
    {
        var response = await _httpClient.DeleteAsync(ApiUrl + $"/api/TagApi/{id}");
        response.EnsureSuccessStatusCode();
    }
}