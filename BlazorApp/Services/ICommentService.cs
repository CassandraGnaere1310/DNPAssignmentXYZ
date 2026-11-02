using ApiContracts;

namespace BlazorApp.Services;

public interface ICommentService
{
    Task<List<CommentDto>> GetCommentsAsync(int postId);
    Task<CommentDto> AddCommentAsync(CreateCommentDto request);
}