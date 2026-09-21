using RepositoryContracts;

namespace CLI.UI;

public class CliApp
{
    private readonly IUserRepository userRepository;
    private readonly ICommentRepository commentRepository;
    private readonly IPostRepository postRepository;

    public CliApp(
        IUserRepository userRepository,
        ICommentRepository commentRepository,
        IPostRepository postRepository)
    {
        this.userRepository = userRepository;
        this.commentRepository = commentRepository;
        this.postRepository = postRepository;
    }

    public async Task StartAsync()
    {
        bool running = true;

        while (running)
        {
            Console.WriteLine();
            Console.WriteLine("===== FORUM MENU =====");
            Console.WriteLine("1. Create user");
            Console.WriteLine("2. Create post");
            Console.WriteLine("3. Add comment");
            Console.WriteLine("4. View posts");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    CreateUserView createUserView =
                        new CreateUserView(userRepository);

                    await createUserView.ShowAsync();
                    break;

                case "2":
                    CreatePostView createPostView =
                        new CreatePostView(postRepository);

                    await createPostView.ShowAsync();
                    break;

                case "3":
                    CreateCommentView createCommentView =
                        new CreateCommentView(commentRepository);

                    await createCommentView.CreateCommentAsync();
                    break;

                case "4":
                    ListPostsView listPostsView =
                        new ListPostsView(
                            postRepository,
                            commentRepository,
                            userRepository);

                    await listPostsView.ShowAsync();
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("Closing application...");
                    break;

                default:
                    Console.WriteLine(
                        "Invalid option, please try again.");
                    break;
            }
        }
    }
}