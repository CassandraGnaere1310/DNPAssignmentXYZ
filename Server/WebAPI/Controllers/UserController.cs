using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public UsersController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    // POST /users
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(
        [FromBody] CreateUserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required.");
        }

        bool usernameTaken = userRepository.GetManyAsync()
            .AsEnumerable()
            .Any(u => string.Equals(
                u.Username,
                request.Username,
                StringComparison.OrdinalIgnoreCase));

        if (usernameTaken)
        {
            return Conflict("Username is already in use.");
        }

        User user = new()
        {
            Username = request.Username,
            Password = request.Password
        };

        User created = await userRepository.AddAsync(user);

        UserDto dto = new()
        {
            Id = created.Id,
            Username = created.Username
        };

        return CreatedAtAction(
            nameof(GetSingle),
            new { id = dto.Id },
            dto);
    }

    // PUT /users/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateUserDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required.");
        }

        User? user = userRepository.GetManyAsync()
            .FirstOrDefault(u => u.Id == id);

        if (user is null)
        {
            return NotFound("User not found.");
        }

        bool usernameTaken = userRepository.GetManyAsync()
            .AsEnumerable()
            .Any(u => u.Id != id && string.Equals(
                u.Username,
                request.Username,
                StringComparison.OrdinalIgnoreCase));

        if (usernameTaken)
        {
            return Conflict("Username is already in use.");
        }

        user.Username = request.Username;
        user.Password = request.Password;

        await userRepository.UpdateAsync(user);

        return NoContent();
    }

    // GET /users/1
    [HttpGet("{id:int}")]
    public async Task<ActionResult<UserDto>> GetSingle(int id)
    {
        bool exists = userRepository.GetManyAsync()
            .Any(u => u.Id == id);

        if (!exists)
        {
            return NotFound("User not found.");
        }

        User user = await userRepository.GetSingleAsync(id);

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username
        });
    }

    // GET /users?username=cas
    [HttpGet]
    public ActionResult<List<UserDto>> GetMany(
        [FromQuery] string? username)
    {
        IEnumerable<User> users = userRepository.GetManyAsync()
            .AsEnumerable();

        if (!string.IsNullOrWhiteSpace(username))
        {
            users = users.Where(u => u.Username.Contains(
                username,
                StringComparison.OrdinalIgnoreCase));
        }

        List<UserDto> result = users.Select(u => new UserDto
        {
            Id = u.Id,
            Username = u.Username
        }).ToList();

        return Ok(result);
    }

    // DELETE /users/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        bool exists = userRepository.GetManyAsync()
            .Any(u => u.Id == id);

        if (!exists)
        {
            return NotFound("User not found.");
        }

        await userRepository.DeleteAsync(id);

        return NoContent();
    }
}