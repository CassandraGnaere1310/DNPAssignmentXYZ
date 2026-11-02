using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpPostService : IPostService
{
    private readonly HttpClient client;

    public HttpPostService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<List<PostDto>> GetPostsAsync()
    {
        using HttpResponseMessage response =
            await client.GetAsync("posts");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<List<PostDto>>()
               ?? throw new Exception("The API did not return a post list.");
    }

    public async Task<PostDto> GetPostAsync(int id)
    {
        using HttpResponseMessage response =
            await client.GetAsync($"posts/{id}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<PostDto>()
               ?? throw new Exception("The API did not return a post.");
    }

    public async Task<PostDto> AddPostAsync(CreatePostDto request)
    {
        using HttpResponseMessage response =
            await client.PostAsJsonAsync("posts", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<PostDto>()
               ?? throw new Exception("The API did not return a post.");
    }
}