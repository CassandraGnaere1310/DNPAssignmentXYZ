using ApiContracts;

namespace BlazorApp.Services;

public interface IPostService
{
    Task<List<PostDto>> GetPostsAsync();
    Task<PostDto> GetPostAsync(int id);
    Task<PostDto> AddPostAsync(CreatePostDto request);
}