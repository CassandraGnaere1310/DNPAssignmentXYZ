using FileRepositories;
using RepositoryContracts;

var builder = WebApplication.CreateBuilder(args);

// Tilføjer support til controllers
builder.Services.AddControllers();

// Registrerer repositories
builder.Services.AddScoped<IUserRepository, UserFileRepository>();
builder.Services.AddScoped<IPostRepository, PostFileRepository>();
builder.Services.AddScoped<ICommentRepository, CommentFileRepository>();

// Tilføjer OpenAPI-dokumentation
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// Gør controller-endpoints tilgængelige
app.MapControllers();

app.Run();