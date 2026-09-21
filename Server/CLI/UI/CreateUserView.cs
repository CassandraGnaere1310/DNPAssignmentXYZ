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

    public Task ShowAsync()
    {
        Console.WriteLine();
        return CreateUserAsync();
    }

    private Task CreateUserAsync()
    {
        Console.WriteLine("You are creating a user.");
        Console.WriteLine("Enter < at any time to cancel.");

        // USERNAME
        string? username;

        while (true)
        {
            Console.WriteLine("Please insert user name:");
            username = Console.ReadLine();

            if (username == "<")
            {
                Console.WriteLine("User creation cancelled.");
                return Task.CompletedTask;
            }

            if (string.IsNullOrWhiteSpace(username))
            {
                Console.WriteLine("Name cannot be empty.");
                continue;
            }

            break;
        }

        // PASSWORD
        string? password;

        while (true)
        {
            Console.WriteLine("Please insert password:");
            password = Console.ReadLine();

            if (password == "<")
            {
                Console.WriteLine("User creation cancelled.");
                return Task.CompletedTask;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("Password cannot be empty.");
                continue;
            }

            break;
        }

        // CONFIRMATION
        Console.WriteLine();
        Console.WriteLine(
            "You are about to create a user with the following information:");

        Console.WriteLine($"User name: {username}");
        Console.WriteLine($"Password: {password}");
        Console.WriteLine("Do you want to proceed? (y/n)");

        while (true)
        {
            string? confirmation =
                Console.ReadLine()?.ToLower();

            switch (confirmation)
            {
                case "y":
                    return AddUserAsync(username, password);

                case "n":
                    Console.WriteLine("User creation cancelled.");
                    return Task.CompletedTask;

                default:
                    Console.WriteLine(
                        "Please select a valid option (y/n).");
                    break;
            }
        }
    }

    private async Task AddUserAsync(
        string username,
        string password)
    {
        User user = new User
        {
            Username = username,
            Password = password
        };

        User created =
            await userRepository.AddAsync(user);

        Console.WriteLine("User created successfully:");
        Console.WriteLine($"ID: {created.Id}");
    }
}