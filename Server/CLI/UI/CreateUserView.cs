using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CreateUserView
{
    private readonly IUserRepository userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task CreateUserAsync()
    {
        Console.Write("Username: ");
        string username = Console.ReadLine() ?? "";

        Console.Write("Password: ");
        string password = Console.ReadLine() ?? "";

        User user = new User
        {
            Username = username,
            Password = password
        };

        User createdUser = await userRepository.AddAsync(user);

        Console.WriteLine($"User created with ID: {createdUser.Id}");
    }
}