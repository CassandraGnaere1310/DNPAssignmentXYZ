using System.Net.Http.Json;
using ApiContracts;

namespace BlazorApp.Services;

public class HttpCommentService : ICommentService
{
    private readonly HttpClient client;

    public HttpCommentService(HttpClient client)
    {
        this.client = client;
    }

    public async Task<List<CommentDto>> GetCommentsAsync(int postId)
    {
        using HttpResponseMessage response =
            await client.GetAsync($"comments?postId={postId}");

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<List<CommentDto>>()
               ?? throw new Exception("The API did not return a comment list.");
    }

    public async Task<CommentDto> AddCommentAsync(CreateCommentDto request)
    {
        using HttpResponseMessage response =
            await client.PostAsJsonAsync("comments", request);

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                await response.Content.ReadAsStringAsync());
        }

        return await response.Content.ReadFromJsonAsync<CommentDto>()
               ?? throw new Exception("The API did not return a comment.");
    }
}