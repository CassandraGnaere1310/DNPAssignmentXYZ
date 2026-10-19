using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("posts")]
public class PostsController : ControllerBase
{
    private readonly IPostRepository postRepository;
    private readonly IUserRepository userRepository;

    public PostsController(
        IPostRepository postRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.userRepository = userRepository;
    }

    // POST /posts
    [HttpPost]
    public async Task<ActionResult<PostDto>> Create(
        [FromBody] CreatePostDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Title and body are required.");
        }

        bool userExists = userRepository.GetManyAsync()
            .Any(u => u.Id == request.UserId);

        if (!userExists)
        {
            return BadRequest("The specified user does not exist.");
        }

        Post post = new()
        {
            Title = request.Title,
            Body = request.Body,
            UserId = request.UserId
        };

        Post created = await postRepository.AddAsync(post);
        PostDto dto = ToDto(created);

        return CreatedAtAction(
            nameof(GetSingle),
            new { id = dto.Id },
            dto);
    }

    // PUT /posts/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdatePostDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Title) ||
            string.IsNullOrWhiteSpace(request.Body))
        {
            return BadRequest("Title and body are required.");
        }

        Post? post = postRepository.GetManyAsync()
            .FirstOrDefault(p => p.Id == id);

        if (post is null)
        {
            return NotFound("Post not found.");
        }

        post.Title = request.Title;
        post.Body = request.Body;

        await postRepository.UpdateAsync(post);

        return NoContent();
    }

    // GET /posts/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PostDto>> GetSingle(int id)
    {
        bool exists = postRepository.GetManyAsync()
            .Any(p => p.Id == id);

        if (!exists)
        {
            return NotFound("Post not found.");
        }

        Post post = await postRepository.GetSingleAsync(id);

        return Ok(ToDto(post));
    }

    // GET /posts?title=hello&userId=1
    [HttpGet]
    public ActionResult<List<PostDto>> GetMany(
        [FromQuery] string? title,
        [FromQuery] int? userId)
    {
        IEnumerable<Post> posts = postRepository.GetManyAsync()
            .AsEnumerable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            posts = posts.Where(p => p.Title.Contains(
                title,
                StringComparison.OrdinalIgnoreCase));
        }

        if (userId.HasValue)
        {
            posts = posts.Where(p => p.UserId == userId.Value);
        }

        List<PostDto> result = posts.Select(ToDto).ToList();

        return Ok(result);
    }

    // DELETE /posts/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool exists = postRepository.GetManyAsync()
            .Any(p => p.Id == id);

        if (!exists)
        {
            return NotFound("Post not found.");
        }

        await postRepository.DeleteAsync(id);

        return NoContent();
    }

    private static PostDto ToDto(Post post)
    {
        return new PostDto
        {
            Id = post.Id,
            Title = post.Title,
            Body = post.Body,
            UserId = post.UserId
        };
    }
}