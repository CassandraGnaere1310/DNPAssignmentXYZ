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

    public async Task CreatePostAsync()
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";

        Console.Write("Body: ");
        string body = Console.ReadLine() ?? "";

        Console.Write("User ID: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        Post post = new Post
        {
            Title = title,
            Body = body,
            UserId = userId
        };

        Post createdPost = await postRepository.AddAsync(post);

        Console.WriteLine($"Post created with ID: {createdPost.Id}");
    }
}