using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class ListPostsView
{
    private readonly IPostRepository postRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IUserRepository userRepository;

    public ListPostsView(
        IPostRepository postRepository,
        ICommentRepository commentRepository,
        IUserRepository userRepository)
    {
        this.postRepository = postRepository;
        this.commentRepository = commentRepository;
        this.userRepository = userRepository;
    }

    public Task ShowAsync()
    {
        Console.WriteLine();
        return ViewPostsAsync();
    }

    private async Task ViewPostsAsync()
    {
        List<Post> posts = postRepository
            .GetManyAsync()
            .OrderBy(p => p.Id)
            .ToList();

        Console.WriteLine("Showing posts:");
        Console.WriteLine("[");

        foreach (Post post in posts)
        {
            Console.WriteLine($"\t({post.Id}): {post.Title}");
        }

        Console.WriteLine("]");

        const string options = """
                               [post id]) View post by id
                               <) Back
                               """;

        Console.WriteLine(options);

        while (true)
        {
            string? input = Console.ReadLine();

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please select a valid option.");
                continue;
            }

            if ("<".Equals(input))
            {
                return;
            }

            if (int.TryParse(input, out int postId))
            {
                ViewPostView viewPostView =
                    new ViewPostView(
                        postRepository,
                        commentRepository);

                await viewPostView.ShowPostAsync(postId);

                Console.WriteLine();
                Console.WriteLine(options);
            }
            else
            {
                Console.WriteLine(
                    "Invalid option, please try again.");
            }
        }
    }
}