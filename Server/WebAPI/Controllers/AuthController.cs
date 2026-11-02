using ApiContracts;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IUserRepository userRepository;

    public AuthController(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    [HttpPost("login")]
    public ActionResult<UserDto> Login([FromBody] LoginRequest request)
    {
        var user = userRepository.GetManyAsync()
            .FirstOrDefault(u => u.Username == request.UserName);

        if (user == null || user.Password != request.Password)
        {
            return Unauthorized("Forkert brugernavn eller adgangskode.");
        }

        return Ok(new UserDto
        {
            Id = user.Id,
            Username = user.Username
        });
    }
}