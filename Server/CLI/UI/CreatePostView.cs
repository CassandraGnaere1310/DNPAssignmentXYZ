using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CreatePostView
{
    private readonly IPostRepository postRepository;

    public CreatePostView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public Task ShowAsync()
    {
        Console.WriteLine();
        return CreatePostAsync();
    }

    private Task CreatePostAsync()
    {
        Console.WriteLine("You are creating a post.");
        Console.WriteLine("Enter < at any time to cancel.");

        // TITLE
        string? title;

        while (true)
        {
            Console.WriteLine("Please insert post title:");
            title = Console.ReadLine();

            if (title == "<")
            {
                Console.WriteLine("Post creation cancelled.");
                return Task.CompletedTask;
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Title cannot be empty.");
                continue;
            }

            break;
        }

        // BODY
        string? body;

        while (true)
        {
            Console.WriteLine("Please insert post content:");
            body = Console.ReadLine();

            if (body == "<")
            {
                Console.WriteLine("Post creation cancelled.");
                return Task.CompletedTask;
            }

            if (string.IsNullOrWhiteSpace(body))
            {
                Console.WriteLine("Content cannot be empty.");
                continue;
            }

            break;
        }

        // USER ID
        int userId;

        while (true)
        {
            Console.WriteLine(
                "Please insert the ID of the user that created the post:");

            string? input = Console.ReadLine();

            if (input == "<")
            {
                Console.WriteLine("Post creation cancelled.");
                return Task.CompletedTask;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("ID cannot be empty.");
                continue;
            }

            if (int.TryParse(input, out userId))
            {
                break;
            }

            Console.WriteLine(
                "Could not parse the ID, please try again.");
        }

        // CONFIRMATION
        Console.WriteLine();
        Console.WriteLine("You are about to create a post.");
        Console.WriteLine($"Title: {title}");
        Console.WriteLine($"Content: {body}");
        Console.WriteLine($"User ID: {userId}");
        Console.WriteLine("Do you want to proceed? (y/n)");

        while (true)
        {
            string? confirmation = Console.ReadLine()?.ToLower();

            switch (confirmation)
            {
                case "y":
                    return AddPostAsync(title, body, userId);

                case "n":
                    Console.WriteLine("Post creation cancelled.");
                    return Task.CompletedTask;

                default:
                    Console.WriteLine(
                        "Please select a valid option (y/n).");
                    break;
            }
        }
    }

    private async Task AddPostAsync(
        string title,
        string body,
        int userId)
    {
        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post added = await postRepository.AddAsync(post);

        Console.WriteLine(
            $"Post created successfully, with ID: {added.Id}");
    }
}