using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class CreateCommentView
{
    private readonly ICommentRepository commentRepository;

    public CreateCommentView(ICommentRepository commentRepository)
    {
        this.commentRepository = commentRepository;
    }

    public async Task CreateCommentAsync()
    {
        Console.Write("Comment: ");
        string body = Console.ReadLine() ?? "";

        Console.Write("User ID: ");
        int userId = int.Parse(Console.ReadLine() ?? "0");

        Console.Write("Post ID: ");
        int postId = int.Parse(Console.ReadLine() ?? "0");

        Comment comment = new Comment
        {
            Body = body,
            UserId = userId,
            PostId = postId
        };

        Comment createdComment =
            await commentRepository.AddAsync(comment);

        Console.WriteLine(
            $"Comment created with ID: {createdComment.Id}");
    }
}