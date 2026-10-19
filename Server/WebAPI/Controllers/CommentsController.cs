using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("comments")]
public class CommentsController : ControllerBase
{
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;
    private readonly IPostRepository postRepository;

    public CommentsController(
        ICommentRepository commentRepository,
        IUserRepository userRepository,
        IPostRepository postRepository)
    {
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
        this.postRepository = postRepository;
    }

    // POST /comments
    [HttpPost]
    public async Task<ActionResult<CommentDto>> Create(
        [FromBody] CreateCommentDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Body is required.");
        }

        bool userExists = userRepository.GetManyAsync()
            .Any(u => u.Id == request.UserId);

        if (!userExists)
        {
            return BadRequest("The specified user does not exist.");
        }

        bool postExists = postRepository.GetManyAsync()
            .Any(p => p.Id == request.PostId);

        if (!postExists)
        {
            return BadRequest("The specified post does not exist.");
        }

        Comment comment = new()
        {
            Body = request.Body,
            UserId = request.UserId,
            PostId = request.PostId
        };

        Comment created = await commentRepository.AddAsync(comment);
        CommentDto dto = ToDto(created);

        return CreatedAtAction(
            nameof(GetSingle),
            new { id = dto.Id },
            dto);
    }

    // PUT /comments/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCommentDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Body is required.");
        }

        Comment? comment = commentRepository.GetManyAsync()
            .FirstOrDefault(c => c.Id == id);

        if (comment is null)
        {
            return NotFound("Comment not found.");
        }

        comment.Body = request.Body;

        await commentRepository.UpdateAsync(comment);

        return NoContent();
    }

    // GET /comments/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<CommentDto>> GetSingle(int id)
    {
        bool exists = commentRepository.GetManyAsync()
            .Any(c => c.Id == id);

        if (!exists)
        {
            return NotFound("Comment not found.");
        }

        Comment comment = await commentRepository.GetSingleAsync(id);

        return Ok(ToDto(comment));
    }

    // GET /comments?userId=1&postId=1
    [HttpGet]
    public ActionResult<List<CommentDto>> GetMany(
        [FromQuery] int? userId,
        [FromQuery] int? postId)
    {
        IQueryable<Comment> comments = commentRepository.GetManyAsync();

        if (userId.HasValue)
        {
            comments = comments.Where(c => c.UserId == userId.Value);
        }

        if (postId.HasValue)
        {
            comments = comments.Where(c => c.PostId == postId.Value);
        }

        List<CommentDto> result = comments
            .AsEnumerable()
            .Select(ToDto)
            .ToList();

        return Ok(result);
    }

    // DELETE /comments/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool exists = commentRepository.GetManyAsync()
            .Any(c => c.Id == id);

        if (!exists)
        {
            return NotFound("Comment not found.");
        }

        await commentRepository.DeleteAsync(id);

        return NoContent();
    }

    private static CommentDto ToDto(Comment comment)
    {
        return new CommentDto
        {
            Id = comment.Id,
            Body = comment.Body,
            UserId = comment.UserId,
            PostId = comment.PostId
        };
    }
}