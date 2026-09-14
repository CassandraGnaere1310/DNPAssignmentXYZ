using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class ViewPostView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;

    public ViewPostView(
        IPostRepository postRepository,
        ICommentRepository commentRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
    }

    public async Task ShowPostAsync()
    {
        Console.Write("Enter post ID: ");

        int postId = int.Parse(
            Console.ReadLine() ?? "0");

        Post post =
            await postRepository.GetSingleAsync(postId);

        Console.WriteLine();
        Console.WriteLine($"Title: {post.Title}");
        Console.WriteLine($"Body: {post.Body}");

        Console.WriteLine();
        Console.WriteLine("Comments:");

        List<Comment> comments = commentRepository
            .GetManyAsync()
            .Where(c => c.PostId == postId)
            .ToList();

        if (comments.Count == 0)
        {
            Console.WriteLine("No comments.");
            return;
        }

        foreach (Comment comment in comments)
        {
            Console.WriteLine(
                $"- {comment.Body} (User ID: {comment.UserId})");
        }
    }
}