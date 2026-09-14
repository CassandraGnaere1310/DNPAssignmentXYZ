using Entities;
using RepositoryContracts;

namespace CLI.UI;

public class ListPostsView
{
    private readonly IPostRepository postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        this.postRepository = postRepository;
    }

    public void ShowPosts()
    {
        List<Post> posts = postRepository
            .GetManyAsync()
            .ToList();

        Console.WriteLine();
        Console.WriteLine("===== POSTS =====");
        Console.WriteLine($"Number of posts: {posts.Count}");

        foreach (Post post in posts)
        {
            Console.WriteLine(
                $"ID: {post.Id} | Title: {post.Title}");
        }
    }
}